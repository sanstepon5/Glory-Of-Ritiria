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


file                :   {StelSysGen.pt(StellarGeneratorPoint.INITMAP);} (stellar_system)+ 
                        {StelSysGen.pt(StellarGeneratorPoint.ERRORVERIFICATION);} EOF ;
stellar_system      :   'stellar_system' id {StelSysGen.pt(StellarGeneratorPoint.INITSYSTEM);} 
                        '{'  
                        stellar_system_body
                        '}' {StelSysGen.pt(StellarGeneratorPoint.ADDSYSTEM);} 
                        ;
stellar_system_body :   name {StelSysGen.pt(StellarGeneratorPoint.SETSYSTEMNAME);} 
                        distance_from 
                        angle 
                        pull
                        (star {StelSysGen.pt(StellarGeneratorPoint.ADDSTAR);})+ ;



star                : 'star' id {StelSysGen.pt(StellarGeneratorPoint.INITSTAR);}  
                       '{' star_body '}' 
                       ;
star_body           : name {StelSysGen.pt(StellarGeneratorPoint.SETSTARNAME);} 
                      (star_class)?  
                      (discovery_status)?
                      (celestial_body {StelSysGen.pt(StellarGeneratorPoint.ADDBODYTOSTAR);})* 
                      (ships)*;



celestial_body      : 'celestial_body' id {StelSysGen.pt(StellarGeneratorPoint.INITBODY);}  
                      '{' celestial_body_body '}'
                      ;
celestial_body_body : name {StelSysGen.pt(StellarGeneratorPoint.SETBODYNAME);} 
                      (body_distance)? 
                      (icon {StelSysGen.pt(StellarGeneratorPoint.SETBODYICON);})?  
                      (discovery_status)?
                      (body_type {StelSysGen.pt(StellarGeneratorPoint.SETBODYTYPE);})?
                      (satellites {StelSysGen.pt(StellarGeneratorPoint.SETISNTSATELLITE);})*  
                      (shipyards)*  
                      (ships)* 
                      ;


satellites          : 'satellites' {StelSysGen.pt(StellarGeneratorPoint.SETISSATELLITE);} '{' 
                      (celestial_body {StelSysGen.pt(StellarGeneratorPoint.ADDSATELLITETOPARENT);})+ 
                      '}' 
                      ;

shipyards           : 'shipyards' '{' (shipyard {StelSysGen.pt(StellarGeneratorPoint.ADDSHIPYARD);})+ '}' ;
shipyard            : 'shipyard' id
                      '{' shipyard_body '}' 
                      ;
shipyard_body       : name {StelSysGen.pt(StellarGeneratorPoint.INITSHIPYARD);} 
                      ( {StelSysGen.pt(StellarGeneratorPoint.SETSHIPYARDBUSY);} ship {StelSysGen.pt(StellarGeneratorPoint.ADDTOSHIPYARD);} )? 
                      ;

ships               : 'ships' '{' (ship {StelSysGen.pt(StellarGeneratorPoint.ADDSHIP);})* '}' ;
ship                : 'ship' id '{' ship_body '}' ;
ship_body           : name {StelSysGen.pt(StellarGeneratorPoint.INITSHIP);} 
                      (building_progress {StelSysGen.pt(StellarGeneratorPoint.SETSHIPYARDBUILDINGPROGRESS);})? 
                      (modules)*  
                      ;
modules             : 'modules' '{' (module {StelSysGen.pt(StellarGeneratorPoint.ADDMODULE);})+ '}' ;
module              : 'module' id {StelSysGen.pt(StellarGeneratorPoint.INITMODULE);} 
                      ('{' 
                        'durability' ':' inty {StelSysGen.pt(StellarGeneratorPoint.SETMODULEDURABILITY);} 
                      '}')? ;



name                : 'name'                    ':' text;
icon                : 'icon'                    ':' text;
body_type           : 'type'                    ':' id;
distance_from       : 'distance_from_detnura'   ':' floaty {StelSysGen.pt(StellarGeneratorPoint.SETSYSTEMDISTANCE);};
angle               : 'map_angle'               ':' inty {StelSysGen.pt(StellarGeneratorPoint.SETSYSTEMANGLE);} ;
pull                : 'gravitational_pull'      ':' inty {StelSysGen.pt(StellarGeneratorPoint.SETSYSTEMPULL);};
body_distance       : 'distance'                ':' inty {StelSysGen.pt(StellarGeneratorPoint.SETBODYDISTANCE);};
building_progress   : 'building_progress'       ':' inty;
star_class          : 'star_class'              ':'( 'orange_dwarf' {StelSysGen.pt(StellarGeneratorPoint.SETSTARCLASSOD);} 
                                                   | 'red_dwarf' {StelSysGen.pt(StellarGeneratorPoint.SETSTARCLASSRD);}
                                                   | 'yellow_dwarf' {StelSysGen.pt(StellarGeneratorPoint.SETSTARCLASSYD);} 
                                                   ) ;
discovery_status    : 'discovery_status'        ':'( 'explored' {StelSysGen.pt(StellarGeneratorPoint.SETSTATUSEXPLORED);}
                                                   | 'existence_known' {StelSysGen.pt(StellarGeneratorPoint.SETSTATUSKNOWN);}
                                                   | 'undiscovered' {StelSysGen.pt(StellarGeneratorPoint.SETSTATUSUNDISCOVERED);}
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

