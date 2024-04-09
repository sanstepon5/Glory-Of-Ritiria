grammar StellarGenerator;

/*
* Parser rules


options {
  language=CSharp;
}
*/

file                :   {Test.pt(StellarGeneratorPoints.INITMAP);} (stellar_system)+ EOF ;
stellar_system      :   'stellar_system' id {Test.pt(StellarGeneratorPoints.INITSYSTEM);} 
                        '{' stellar_system_body '}' ;
stellar_system_body :   name {Test.pt(StellarGeneratorPoints.SETSYSTEMNAME);} NEWLINE 
                        distance_from NEWLINE angle NEWLINE pull NEWLINE (star)+ ;



star                : 'star' id '{' star_body '}' ;
star_body           : name NEWLINE (star_class NEWLINE)?  (discovery_status NEWLINE)?  (celestial_body NEWLINE)+ (ships)*;



celestial_body      : 'celestial_body' id '{' celestial_body_body '}' ;
celestial_body_body : name NEWLINE body_distance NEWLINE (icon NEWLINE)?  
                      (discovery_status NEWLINE)?  (satellites NEWLINE)*  (shipyards NEWLINE)*  (ships)* ;


satellites          : 'satellites' '{' (celestial_body NEWLINE)+ '}' ;

shipyards           : 'shipyards' '{' (shipyard NEWLINE)+ '}' ;
shipyard            : 'shipyard' id '{' shipyard_body '}' ;
shipyard_body       : name NEWLINE (ship)? ;

ships               : 'ships' '{' (ship NEWLINE)+ '}' ;
ship                : 'ship' id '{' ship_body '}' ;
ship_body           : name NEWLINE (building_progress NEWLINE)? (modules)*  ;
modules             : 'modules' '{' (module NEWLINE)+ '}' ;
module              : id ('{' 'durability' ':' inty '}')? ;



name                : NAME ;
icon                : TEXT ;
distance_from       : 'distance_from_detnura'   ':' floaty;
body_distance       : 'distance'                ':' inty;
angle               : 'map_angle'               ':' inty;
pull                : 'gravitational_pull'      ':' inty;
building_progress   : 'building_progress'       ':' inty;
star_class          : 'star_class'              ':'( 'orange_dwarf' | 'red_dwarf' | 'yellow_dwarf' ) ;
discovery_status    : 'discovery_status'        ':'( 'explored' | 'existence_known' | 'undiscovered' ) ;

id                   : ID {Test.CurrentText=$ID.text;};
inty                 : INT {Test.CurrentInt = int.Parse($INT.text);};
floaty               : FLOAT {Test.CurrentFloat = float.Parse($INT.text);};


fragment A          : ('A'|'a') ;
fragment S          : ('S'|'s') ;
fragment Y          : ('Y'|'y') ;
fragment UNDERSCORE : '_' ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment NUMBER     : [0-9] ;
INT                 : '0'..'9'+ ;
FLOAT               : (INT)+('.'INT)* ;
SAYS                : S A Y S ;

WORD                : (LOWERCASE | UPPERCASE | UNDERSCORE)+ ;
ID                  : (LOWERCASE)(LOWERCASE | UNDERSCORE | NUMBER)* ;
NAME                : (WORD)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t')+ -> skip ;
COMMENT             : '#' ~( '\r' | '\n' )* -> skip ; // Everything following # is a comment

NEWLINE             : ('\r'? '\n' | '\r')+ ;

