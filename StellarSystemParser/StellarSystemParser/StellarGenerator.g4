grammar StellarGenerator;

/*
* Parser rules


options {
  language=CSharp;
}
*/
@header {
    using System.Globalization;
    using GloryOfRitiria.Scripts.Utils;
}


file                :   {StelSysGen.pt(StellarGeneratorPoint.Initmap);} (stellar_system)+ 
                        {StelSysGen.pt(StellarGeneratorPoint.Errorverification);} EOF ;
stellar_system      :   'stellar_system' id {StelSysGen.pt(StellarGeneratorPoint.Initsystem);} 
                        '{'  
                        stellar_system_body
                        '}' {StelSysGen.pt(StellarGeneratorPoint.Addsystem);} 
                        ;
stellar_system_body :   name {StelSysGen.pt(StellarGeneratorPoint.Setsystemname);} 
                        distance_from 
                        angle 
                        pull
                        (star {StelSysGen.pt(StellarGeneratorPoint.addstar);})+ ;



star                : 'star' id {StelSysGen.pt(StellarGeneratorPoint.Initstar);}  
                       '{' star_body '}' 
                       ;
star_body           : name {StelSysGen.pt(StellarGeneratorPoint.Setstarname);} 
                      (star_class)?  
                      (discovery_status)?
                      (celestial_body {StelSysGen.pt(StellarGeneratorPoint.Addbodytostar);})* 
                      (ships)*;



celestial_body      : 'celestial_body' id {StelSysGen.pt(StellarGeneratorPoint.Initbody);}  
                      '{' celestial_body_body '}'
                      ;
celestial_body_body : name {StelSysGen.pt(StellarGeneratorPoint.Setbodyname);} 
                      (body_distance)? 
                      (icon {StelSysGen.pt(StellarGeneratorPoint.Setbodyicon);})?  
                      (discovery_status)?
                      (body_type {StelSysGen.pt(StellarGeneratorPoint.Setbodytype);})?
                      (satellites {StelSysGen.pt(StellarGeneratorPoint.Setisntsatellite);})*  
                      (shipyards)*  
                      (ships)* 
                      ;


satellites          : 'satellites' {StelSysGen.pt(StellarGeneratorPoint.Setissatellite);} '{' 
                      (celestial_body {StelSysGen.pt(StellarGeneratorPoint.Addsatellitetoparent);})+ 
                      '}' 
                      ;

shipyards           : 'shipyards' '{' (shipyard {StelSysGen.pt(StellarGeneratorPoint.Addshipyard);})+ '}' ;
shipyard            : 'shipyard' id
                      '{' shipyard_body '}' 
                      ;
shipyard_body       : name {StelSysGen.pt(StellarGeneratorPoint.Initshipyard);} 
                      ( {StelSysGen.pt(StellarGeneratorPoint.Setshipyardbusy);} ship {StelSysGen.pt(StellarGeneratorPoint.Addtoshipyard);} )? 
                      ;

ships               : 'ships' '{' (ship {StelSysGen.pt(StellarGeneratorPoint.Addship);})* '}' ;
ship                : 'ship' id '{' ship_body '}' ;
ship_body           : name {StelSysGen.pt(StellarGeneratorPoint.Initship);} 
                      (building_progress {StelSysGen.pt(StellarGeneratorPoint.Setshipyardbuildingprogress);})? 
                      (modules)*  
                      ;
modules             : 'modules' '{' (module {StelSysGen.pt(StellarGeneratorPoint.Addmodule);})+ '}' ;
module              : 'module' id {StelSysGen.pt(StellarGeneratorPoint.Initmodule);} 
                      ('{' 
                        'durability' ':' inty {StelSysGen.pt(StellarGeneratorPoint.Setmoduledurability);} 
                      '}')? ;



name                : 'name'                    ':' text;
icon                : 'icon'                    ':' text;
body_type           : 'type'                    ':' id;
distance_from       : 'distance_from_detnura'   ':' floaty {StelSysGen.pt(StellarGeneratorPoint.Setsystemdistance);};
angle               : 'map_angle'               ':' inty {StelSysGen.pt(StellarGeneratorPoint.Setsystemangle);} ;
pull                : 'gravitational_pull'      ':' inty {StelSysGen.pt(StellarGeneratorPoint.Setsystempull);};
body_distance       : 'distance'                ':' inty {StelSysGen.pt(StellarGeneratorPoint.Setbodydistance);};
building_progress   : 'building_progress'       ':' inty;
star_class          : 'star_class'              ':'( 'orange_dwarf' {StelSysGen.pt(StellarGeneratorPoint.Setstarclassod);} 
                                                   | 'red_dwarf' {StelSysGen.pt(StellarGeneratorPoint.Setstarclassrd);}
                                                   | 'yellow_dwarf' {StelSysGen.pt(StellarGeneratorPoint.Setstarclassyd);} 
                                                   ) ;
discovery_status    : 'discovery_status'        ':'( 'explored' {StelSysGen.pt(StellarGeneratorPoint.Setstatusexplored);}
                                                   | 'existence_known' {StelSysGen.pt(StellarGeneratorPoint.Setstatusknown);}
                                                   | 'undiscovered' {StelSysGen.pt(StellarGeneratorPoint.Setstatusundiscovered);}
                                                   ) ;

id                   : ID {StelSysGen.CurrentText = $ID.text;};
text                 : TEXT {StelSysGen.CurrentText = $TEXT.text.Replace('"', ' ').Trim();} ;
inty                 : INT {StelSysGen.CurrentInt = int.Parse($INT.text);};
floaty               : FLOAT {StelSysGen.CurrentFloat = float.Parse($FLOAT.text, CultureInfo.InvariantCulture);};


fragment UNDERSCORE : '_' ;
fragment DASH       : '-' ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment NUMBER     : [0-9] ;
INT                 : NUMBER+ ;
FLOAT               : NUMBER+(('.')(NUMBER+)+)? ;

ID                  : ([a-z])([a-z] | '_' | [0-9])* ;
WORD                : (LOWERCASE | UPPERCASE | UNDERSCORE | DASH)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t'|'\n'|'\r')+ -> skip ;
COMMENT             : '#' ~( '\r' | '\n' )* -> skip ; // Everything following # is a comment

NEWLINE             : ('\r'? '\n' | '\r')+ ;

