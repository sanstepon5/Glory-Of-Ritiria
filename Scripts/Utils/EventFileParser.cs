using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GloryOfRitiria.Scripts.Scenes.Parts;
using GloryOfRitiria.Scripts.Utils.Events;
using GloryOfRitiria.Scripts.Utils.Events.Conditions;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;

// TODO: Refactoring everything
// TODO: Make comments work everywhere (inside of an event)...
// TODO: Better errors
// TODO: For event descriptions, add line return (description on multiple lines between quotes)
// To be fair it's not that big of a deal, real text should be in localisation files anyway
// Indentation would be nice even if it kinda works without



public class EventFileParser
{
    private FileAccess _eventFile;
    private string _currentLine;
    
    //Regexes for use in functions
    private Regex _commentRegex;
    private Regex _onlyBlancRegex;
    private Regex _startsByEventRegex;
    private Regex _indentationRegex;
    private Regex _idRegex;
    private Regex _titleRegex;
    private Regex _descriptionRegex;
    private Regex _imageRegex;
    private Regex _conditionRegex;
    private Regex _optionsRegex;
    private Regex _choiceRegex;
    private Regex _effectsRegex;

    public EventFileParser(FileAccess eventFile)
    {
        _eventFile = eventFile;
    }

    private void RegexInit()
    {
        _commentRegex = new Regex(@"^\s*//");
        _onlyBlancRegex = new Regex(@"^\s*$");
        
        _startsByEventRegex = new Regex(@"^Event\s*:\s*$");
        
        // TODO: Think about this one...
        _indentationRegex = new Regex(@"^\t{1}| {4}| {2}");
        
        // Regexes for reading different simple values
        _idRegex = new Regex(@"ID\s*:\s*""(?<ID>.*)""\s*$");
        _titleRegex = new Regex(@"Title\s*:\s*""(?<Title>.*)""\s*$");
        _descriptionRegex = new Regex(@"Description\s*:\s*""(?<Description>.*)""\s*$");
        _imageRegex = new Regex(@"Image\s*:\s*""(?<Image>.*)""\s*$");
        // Condition word followed by it type (For All, Any, Simple or Flag) for example: "Condition For All:"
        _conditionRegex = new Regex(@"Condition\s*(?<Type>For\s*All|Any|Simple|Flags)\s*:\s*$");
        
        _optionsRegex = new Regex(@"Options\s*:\s*$");
        _choiceRegex = new Regex(@"Choice\s*:\s*$");
        _effectsRegex = new Regex(@"Effects\s*:\s*$");
    }


    public List<GameEvent> ReadEventList()
    {
        RegexInit();
        var eventList = new List<GameEvent>();

        _currentLine = _eventFile.GetLine(); // I suppose it returns null at the end...
        
        while (!_eventFile.EofReached())
        {
            // Skip the line if it's commented or only whitespaces
            if (_commentRegex.IsMatch(_currentLine) || _onlyBlancRegex.IsMatch(_currentLine)) 
            {
                _currentLine = _eventFile.GetLine();
                continue;
            }
            if (!_startsByEventRegex.IsMatch(_currentLine)) throw new Exception("Event file syntax wrong");
            
            _currentLine = _eventFile.GetLine();
            
            // ReadEvent will advance current line until it finishes events
            eventList.Add(ReadEvent());
        }
        
        return eventList;
    }

    private GameEvent ReadEvent()
    {
        var gameEvent = new GameEvent();

        while (!string.IsNullOrEmpty(_currentLine) || _indentationRegex.IsMatch(_currentLine))
        {
            // Skip the line if it's commented or only whitespaces
            if (_commentRegex.IsMatch(_currentLine) || _onlyBlancRegex.IsMatch(_currentLine))
            {
                _currentLine = _eventFile.GetLine();
                continue;
            }
            //if (!indentationRegex.IsMatch(_currentLine)) throw new Exception("Indentation error");
            
            // Read and assign value for simple attributes
            var match = _idRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.Id = match.Groups["ID"].Value; 
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = _titleRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.Name = match.Groups["Title"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = _descriptionRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.Description = match.Groups["Description"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = _imageRegex.Match(_currentLine);
            if (match.Success)
            {
                gameEvent.ImagePath = match.Groups["Image"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            
            // Read Conditions and Effects
            match = _conditionRegex.Match(_currentLine);
            if (match.Success)
            {
                _currentLine = _eventFile.GetLine();
                gameEvent.Condition = ReadCondition(match.Groups["Type"].Value, 2);
                continue;
            }
            
            match = _optionsRegex.Match(_currentLine);
            if (match.Success)
            {
                _currentLine = _eventFile.GetLine();
                gameEvent.Options = ReadOptions();
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
        // var nbTabs = indentationLevel * 1;
        // var nbSpaces2 = indentationLevel * 2;
        // var nbTabs4 = indentationLevel * 4;
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
                //var IndentRegex = new Regex(@"^\t{" + indentationLevel + @"}");
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
                
                return condition;
            }
            default:
                throw new Exception("Trying to read unhandled condition type");
        }
    }
    
    private List<Choice> ReadOptions()
    {
        var choicesList = new List<Choice>();
        
        var match = _choiceRegex.Match(_currentLine);
        if (!match.Success) throw new Exception("Options line isn't followed by a Choice line (must have at least one Choice)");

        // Each ReadChoice should position the line either on the next "Choice :" line or something else.
        while (match.Success)
        {
            _currentLine = _eventFile.GetLine();
            choicesList.Add(ReadChoice());
            match = _choiceRegex.Match(_currentLine);
        }
        
        return choicesList;
    }

    private Choice ReadChoice()
    {
        var choice = new Choice();
        
        while (!_eventFile.EofReached())
        {
            // Skip the line if it's commented or only whitespaces
            if (_commentRegex.IsMatch(_currentLine) || _onlyBlancRegex.IsMatch(_currentLine))
            {
                _currentLine = _eventFile.GetLine();
                continue;
            }
            
            // Read and assign value for simple attributes
            var match = _idRegex.Match(_currentLine);
            if (match.Success)
            {
                choice.Id = match.Groups["ID"].Value; 
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = _descriptionRegex.Match(_currentLine);
            if (match.Success)
            {
                choice.Desc = match.Groups["Description"].Value;
                _currentLine = _eventFile.GetLine(); 
                continue;
            }
            
            match = _effectsRegex.Match(_currentLine);
            if (match.Success)
            {
                _currentLine = _eventFile.GetLine(); 
                choice.Effects = ReadEffects();
                
                continue;
            }
            
            // If all possible fields of the choice are done, stop reading
            break;
        }
        
        return choice;
    }

    private List<Effect> ReadEffects()
    {
        var effectsList = new List<Effect>();
        
        var effectValuesRegex = new Regex(@"^\s*(?<MethodName>.*),\s*(?<Value>.*),\s*""(?<Desc>.*)""\s*;\s*$");

        var match = effectValuesRegex.Match(_currentLine);
        if (!match.Success) throw new Exception("Couldn't read effect values");

        while (match.Success)
        {
            var effect = new Effect(match.Groups["MethodName"].Value, 
                                    match.Groups["Value"].Value, match.Groups["Desc"].Value);
            effectsList.Add(effect);
            
            _currentLine = _eventFile.GetLine();
            match = effectValuesRegex.Match(_currentLine);
        }

        return effectsList;
    }
    
}

