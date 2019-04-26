# DataConverter
### Semiconductor Data Conversion Utility

Converts various semiconductor data formats from one to the other, where conversion is possible.
Currently recognized file types to convert / generate:  
- UDF (***U***ncompressed binary ***D***ata ***F***ile)
- CDF (***C***ompressed binary ***D***ata ***F***ile)
- KLARF (***KLA R***esults ***F***ile)
- SINF (***S***implified ***I***ntegrator ***N***ested ***F***ormat)
       
Available conversions:
- UDF -> SINF OR CDF
- CDF -> SINF OR UDF
- KLARF -> SINF

Usage:  

    dataconverter --i ""[input file path]"" --f [input format] --o ""[output directory path]"" --t [output format]
