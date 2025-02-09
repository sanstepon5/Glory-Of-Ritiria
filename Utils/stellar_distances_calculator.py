from typing import Tuple
import numpy as np
from simbad_extractor import SimbadQuerier

def parse_ra(ra_string: str) -> float:
    """Convert Right Ascension from 'hh mm ss' to degrees."""
    ra_string = ra_string.replace('h', '').replace('m', '').replace('s', '').strip()
    h, m, s = [float(i) for i in ra_string.split()]
    return 15 * (h + m / 60 + s / 3600)

def parse_dec(dec_string: str) -> float:
    """Convert Declination from 'dd mm ss' to degrees."""
    dec_string = (dec_string.replace('°', '').replace('′', '').replace('″', '')
                  .replace("+", "").replace("−", "-").strip()) # '−' != '-'
    d, m, s = [float(i) for i in dec_string.split()]
    return d + m / 60 + s / 3600


def spherical_to_cartesian(ra_deg: float, dec_deg: float, distance: float) -> np.array:
    """Function to convert spherical coordinates (RA, Dec, distance) to Cartesian (x, y, z)"""
    ra_rad = np.radians(ra_deg)
    dec_rad = np.radians(dec_deg)

    # Cartesian coordinates
    x = distance * np.cos(ra_rad) * np.cos(dec_rad)
    y = distance * np.sin(ra_rad) * np.cos(dec_rad)
    z = distance * np.sin(dec_rad)
    
    return np.array([x, y, z])

def cartesian_to_spherical(x: float, y: float, z: float, rounding=6) -> Tuple[float, float]:
    """Function to convert Cartesian coordinates (x, y, z) to spherical (RA, distance), rounded to rounding decimals"""
    distance = np.sqrt(x**2 + y**2 + z**2)
    ra_rad = np.arctan2(y, x)
    ra_deg = np.degrees(ra_rad) % 360  # Normalize RA to [0, 360)
    
    return round(float(ra_deg), rounding), round(float(distance), rounding)





stars = ["Barnard's Star", "Luhman 16", "WISE J085510.83−071442.5", "Wolf 359", "Lalande 21185", 
         "Alpha Canis Majoris A", "Alpha Canis Majoris B", "Gliese 65 A", "Gliese 65 B", "Ross 154", "Ross 248", # Canis Majoris = Sirius
         "Ran", "Lacaille 9352", "Ross 128", "EZ Aquarii", # EZ Aquarii is a triple system but no data on simbad 
        "Alpha Canis Minoris A", "Alpha Canis Minoris B", #  Procyon 
        "61 Cygni A", "61 Cygni B", "Gliese 725 A", "Gliese 725 B", # No dedicated page for Gliese 725 separately
         "GX Andromedae", "GQ Andromedae", # System Groombridge 34 (Gliese 15) 
         "DX Cancri", "Epsilon Indi", # Two brown dwarfs in orbit, could be probably be like giants
         "Tau Ceti"]

stars_final = {"Sol": (30, 4.2)}

querier = SimbadQuerier()
ra, dec, dist = querier.query_simbad("Alpha Centauri")
detnura_cartesian = spherical_to_cartesian(parse_ra(ra), parse_dec(dec), dist)
sol_cartesian = spherical_to_cartesian(0, 0, 0) - detnura_cartesian

stars_final["Detnura"] = (0, 0)
stars_final["Sol"] = cartesian_to_spherical(*sol_cartesian)

for star_name in stars:
    star_data = querier.query_simbad(star_name)
    if star_data:
        ra, dec, dist = star_data
        star_cartesian = spherical_to_cartesian(parse_ra(ra), parse_dec(dec), dist)
        print(f"{star_name} From Sol: angle: {(parse_dec(dec), parse_ra(ra))}, distance {dist}")

        # Move the center to Alpha Centauri by subtracting Alpha Centauri's Cartesian coordinates
        star_cartesian = star_cartesian - detnura_cartesian
        stars_final[star_name] = cartesian_to_spherical(*star_cartesian)
    else:
        print(f"{star_name} not found or missing data.")

for key, value in stars_final.items():
    print(f"{key}: {value}")
