# DMIExtractorV2
This tool is simple and requires you to give it 2 filepaths.

Data path is the path you want to extract from.

Output path is the path you want to extract to.

## What the tool does
It first checks the paths you gave it, if it is valid, it will allow extraction.

It also shows a complete folder view of all the files it will extract, you can view it all by expanding the nodes.

Then when you extract its going to go trough a few steps of extractions:
- Initialize all the folders required:
  It will create relative paths from the datapath and then append them to the output path, and create the directory
- Extract dmi files into dmi folders
  This is basically the same as the relative path part, just that it makes the folder with the name of the original dmi file
  Then it extracts all the files from the dmi file
- Copy png files
- Copy gif files

And thats the entire extraction flow
