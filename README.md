# HexMerge
HexMerge lets user merge .HEX files into one.
Main use case is merging bootloader with firmware in one distributable file.

# GUI version
Lets user select single files and trigger merge via button.

![Immagine 2023-07-29 082930](https://github.com/fasterbicio/HexMerge/assets/58078642/f1140e92-03c9-42f6-b7c8-c0a3b103200b)

# CLI version
Can be used in post build steps.

Syntax would be:
```
merge.exe $bootloader$.hex $firmware$.hex $mergedOutput$.hex
```
