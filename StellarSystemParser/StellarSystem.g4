grammar StellarSystem;

/*
* Parser rules
*/
options {
  language=Csharp;
}


file                : (stellar_system)+ EOF ;
stellar_system      : 'stellar_system' id '{' stellar_system_body '}' ;
stellar_system_body : name NEWLINE distance_from NEWLINE angle NEWLINE pull NEWLINE (star)+ ;



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
module              : id ('{' 'durability' ':' int '}')? ;



name                : NAME ;
icon                : TEXT ;
distance_from       : 'distance_from_detnura'   ':' float;
body_distance       : 'distance'                ':' int;
angle               : 'map_angle'               ':' int;
pull                : 'gravitational_pull'      ':' int;
building_progress   : 'building_progress'       ':' int;
star_class          : 'star_class'      ':'(
                                            'orange_dwarf'
                                          | 'red_dwarf'
                                          | 'yellow_dwarf'
                                        ) ;
discovery_status    : 'discovery_status' ':'(
                                            'explored'
                                          | 'existence_known'
                                          | 'undiscovered'
                                        ) ;

id                  : ID {PtGen.currentID=$ID.text;};
int                 : INT {PtGen.currentID=$ID.text;};
float               : FLOAT {PtGen.currentID=$ID.text;};


fragment A          : ('A'|'a') ;
fragment S          : ('S'|'s') ;
fragment Y          : ('Y'|'y') ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
INT                 : '0'..'9'+ ;
FLOAT               : (INT)+('.'INT)* ;
SAYS                : S A Y S ;
UNDERSCORE          : '_' ;
WORD                : (LOWERCASE | UPPERCASE | UNDERSCORE)+ ;
ID                  : ('a'..'z')('a'..'z' | '_'| INT)* ;
NAME                : (WORD)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;

