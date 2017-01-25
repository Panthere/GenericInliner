# GenericInliner
A base for creating inliners for .NET deobfuscation

# Compiling from Source
- Download/Fork
- Open sln
- Rebuild entire solution
- Add folder called 'Plugins' in your output location
- All plugins will be copied to this on build (I advise you to do the same with your plugin project to save time)


# Plugin Support
A plugin must inherit the IGenericInliner class and fill out all relevant details.
You may look at the examples given by 'CallInliner', 'MathInliner', and 'VariableInliner'

You must add reference to the ClientPlugin, and the modified dnlib available in the 'Dependencies' folder.



# Notes
- This is not a final work
- There are some bugs, but very minimal
