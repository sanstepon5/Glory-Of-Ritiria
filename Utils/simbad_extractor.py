from typing import Tuple
from astroquery.simbad import Simbad

class SimbadQuerier:
    def __init__(self) -> None:
        # Custom Simbad query to include the 'plx' field (parallax)
        self.custom_simbad = Simbad()
        self.custom_simbad.add_votable_fields('parallax')  # Request the parallax field

    def query_simbad(self, star_name: str) -> Tuple[str, str, float]:
        result_table = self.custom_simbad.query_object(star_name)
        if result_table is not None:
            ra = result_table['RA'][0]
            dec = result_table['DEC'][0]
            
            # Get parallax in milliarcseconds (mas)
            if 'PLX_VALUE' in result_table.colnames:
                parallax = result_table['PLX_VALUE'][0]
            else:
                return None
            
            distance_pc = 1000 / parallax  # Distance in parsecs (pc)
            distance_ly = distance_pc * 3.26156  # Convert parsecs to light-years
                
            
            return ra, dec, distance_ly
        else:
            return None


