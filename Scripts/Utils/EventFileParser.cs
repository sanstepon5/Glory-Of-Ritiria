using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GloryOfRitiria.Scripts.Scenes.Parts;
using GloryOfRitiria.Scripts.Utils.Events;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;

// Only whole line comments authorized
// TODO: Deal with empty lines between events (both an empty line between lines and the end of file are just "")
// TODO: Add Effects
// TODO: Better Regexes, especially with indent (or maybe just force using keywords?
// TODO: For event descriptions, add line return (description on multiple lines between quotes)
// TODO: Make it cleaner, especially with different Regex patterns

public class EventFileParser
{
    private FileAccess _eventFile;
    private string _currentLine;
    private Regex _commentRegex = new Regex(@"^\s*//$"); 
    private Regex _onlyBlancRegex = new Regex(@"^\s*$");

    public EventFileParser(FileAccess eventFile)
    {
        this._eventFile = eventFile;
    }

    


    public List<GameEvent> ReadEventList()
    {
        var eventList = new List<GameEvent>();

        var startsByEventRegex = new Regex(@"^Event\s*:\s*$");

        _currentLine = _eventFile.GetLine(); // I suppose it returns null at the end...
        
        while (!string.IsNullOrEmpty(_currentLine))
        {
            // Continue reading lines if comments or blanc lines, stop if the line doesn't start by Event:
            if (_commentRegex.IsMatch(_currentLine) || _onlyBlancRegex.IsMatch(_currentLine)) continue;
            if (!startsByEventRegex.IsMatch(_currentLine)) throw new Exception("Event file syntax wrong");
            
            _currentLine = _eventFile.GetLine();
            
            // ReadEvent will advance current line until it finishes events
            eventList.Add(ReadEvent());
        }
        
        return eventList;
    }

    private GameEvent ReadEvent()
    {
        var gameEvent = new GameEvent();
        
        var indentationRegex = new Regex(@"^\t{1}| {4}| {2}"); // indentation is a tab or 2 or 4 spaces
        
        // Indetation should be checked before, capture everything inside of " " thanks to "" (escaped ")
        var idRegex = new Regex(@"ID\s*:\s*""(?<ID>.*)""\s*$");
        var titleRegex = new Regex(@"Title\s*:\s*""(?<Title>.*)""\s*$");
        var descriptionRegex = new Regex(@"Description\s*:\s*""(?<Description>.*)""\s*$");
        var imageRegex = new Regex(@"Image\s*:\s*""(?<Image>.*)""\s*$");
        
        // Condition word followed by it type (For All, Any, Simple or Flag) for example: "Condition For All:"
        var conditionRegex = new Regex(@"Condition\s*(?<Type>For\s*All|Any|Simple|Flags)\s*:\s*$");
        var ChoicesRegex = new Regex(@"Choices\s*:\s*$");


        
        
        while (!string.IsNullOrEmpty(_currentLine) || indentationRegex.IsMatch(_currentLine))
        {
            // Continue reading lines if comments or blanc lines, stop if the line doesn't start by Event:
            if (_commentRegex.IsMatch(_currentLine) || _onlyBlancRegex.IsMatch(_currentLine)) continue;
            //if (!indentationRegex.IsMatch(_currentLine)) throw new Exception("Indentation error");
            
            // Read and assign value for simple attributes
            var match = idRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.Id = match.Groups["ID"].Value; 
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = titleRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.Name = match.Groups["Title"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = descriptionRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.Description = match.Groups["Description"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = imageRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.ImagePath = match.Groups["Image"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            
            // Read Conditions and Effects
            match = conditionRegex.Match(_currentLine);
            if (match.Success)
            {
                _currentLine = _eventFile.GetLine();
                gameEvent.Condition = ReadCondition(match.Groups["Type"].Value, 2);
                continue;
            }
            
            match = ChoicesRegex.Match(_currentLine);
            if (match.Success)
            {
                _currentLine = _eventFile.GetLine();
                gameEvent.Options = ReadChoices();
                continue;
            }
            
            // If all possible fields of the event are done, stop reading
            break;
        }
        
        return gameEvent;
    }

    // How many indentations should the condition values have (for example 2 if the event has a single simple condition
    // Or 3 if the event condition was a For All/Any and you're trying to read a condition of this For All/Any
    private IEventCondition ReadCondition(string conditionType, int indentationLevel)
    {
        var nbTabs = indentationLevel * 1;
        var nbSpaces2 = indentationLevel * 2;
        var nbTabs4 = indentationLevel * 4;
        switch (conditionType)
        {
            case "Simple":
            {
                var condition = new SingleCondition();
                // I trust the user to put the right values between commas......
                var conditionValuesRegex = new Regex(@"^\s*(?<VarName>.*),\s*(?<CondType>.*),\s*(?<CondValue>.*);\s*$");
                // var conditionValuesRegex = new Regex(@"^\t{{{nbTabs}}}| {{{nbSpaces2}}}| {{{nbTabs4}}}
                //                                             (?<VarName>.*),\s*(?<CondType>.*),\s*(?<CondValue>.*);\s*$");
                var match = conditionValuesRegex.Match(_currentLine);
                if (!match.Success) throw new Exception("Couldn't read single condition values");

                condition.VarName = match.Groups["VarName"].Value;
                condition.CondType = match.Groups["CondType"].Value;
                condition.Value = match.Groups["CondValue"].Value;

                _currentLine = _eventFile.GetLine();
                return condition;
            }
                
            case "Flags":
            {
                var condition = new FlagCondition();
                var flagRegex = new Regex(@"(?<FlagName>\w+)\s*$"); //A flag doesn't contain spaces
                //var flagRegex = new Regex(@"^\t{{{nbTabs}}}| {{{nbSpaces2}}}| {{{nbTabs4}}}(?<FlagName>.*)\s*$");
                var match = flagRegex.Match(_currentLine);
                // If first flag can't match there is a problem, next line however can just be the next condition
                if (!match.Success) throw new Exception("Couldn't read flag values");
                
                while (match.Success)
                {
                    condition.AddFlag(match.Groups["FlagName"].Value);
                    _currentLine = _eventFile.GetLine();
                    match = flagRegex.Match(_currentLine);
                }

                return condition;
            }
            
            case "Any":
            {
                var condition = new AnyCondition();
                var conditionRegex = new Regex(@"Condition\s*(?<Type>For\s*All|Any|Simple|Flags)\s*:\s*$");
                var match = conditionRegex.Match(_currentLine);
                // If current line isn't a condition name, there is a problem
                if (!match.Success) throw new Exception("Couldn't read condition type");
                
                while (match.Success)
                {
                    // ReadCondition will advance line until new condition
                    _currentLine = _eventFile.GetLine();
                    condition.AddCondition(ReadCondition(match.Groups["Type"].Value, indentationLevel + 1));
                    match = conditionRegex.Match(_currentLine);
                }
                
                _currentLine = _eventFile.GetLine();
                return condition;
            }
            
            case "For All":
            {
                var condition = new ForAllCondition();
                var conditionRegex = new Regex(@"Condition\s*(?<Type>For\s*All|Any|Simple|Flags)\s*:\s*$");
                var match = conditionRegex.Match(_currentLine);
                // If current line isn't a condition name, there is a problem
                if (!match.Success) throw new Exception("Couldn't read condition type");
                
                while (match.Success)
                {
                    // ReadCondition will advance line until new condition
                    _currentLine = _eventFile.GetLine();
                    condition.AddCondition(ReadCondition(match.Groups["Type"].Value, indentationLevel + 1));
                    match = conditionRegex.Match(_currentLine);
                }
                
                _currentLine = _eventFile.GetLine();
                return condition;
            }
            default:
                throw new Exception("Trying to read unhandled condition type");
        }
    }
    
    private List<Choice> ReadChoices()
    {
        var choicesList = new List<Choice>();
        return choicesList;
    }
    
}



// Event:
//     ID: "event_1"
//     Title: "First Event"
//     Description: "I dunno, just a long text describing stuff here. Or maybe just a path to localisation or something."
//     Image: "Path/To/Image.png. Or something."
//     Condition For All:
//         Condition Simple:
//             Res1, ==, 30
//         Condition Flags:
//             RandomFlag1
//             RandomFlag2
//     Choices:
//         Choice:
//             Id: "Option 1"
//             Desc: "This option adds 10 Res1"
//             Effects: 
//                 "Res1Add": "10"
//                 "AddFlag": "RandomFlag3"
//         Choice:
//             Id: "Option 2"
//             Desc: "This option substructs 10 Res1"
//             Effects: 
//                 "Res1Add": "-10"
//                 "RemoveFlag": "RandomFlag1"
//
// Event: event_2
//     ...
//


