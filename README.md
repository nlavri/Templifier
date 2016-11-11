Templifier
==================================

Lightweight and portable tool that allows you to create a template package.
Created to automate a process of generating Visual Studio solutions. Or any other IDE projects or arbitrary file sets.

For example: 
You create VS solution with all neccessary projects (web/logic/data), all references etc.
Then you specify a token to replace (usually a root namespace) and create a package with the tool. 
Deploy an number of specific solutions in a moment.

Can be useful for microservices etc.

Based on and inspired by https://github.com/endjin/Templify 
but doesn't require installation and uses nuget-only 3-rd party references.

Create Package:

  `Templifier.exe -m c -p C:\MyTemplate.pkg -f "c:\SampleSolutionFolder" -t "SampleBaseNamespace=\__NAME\__"`

Deploy Package:

  `Templifier.exe -m d -p C:\MyTemplate.pkg -f "c:\SolutionBasedOnTemplateFolder" -t "\__NAME\__=NewBaseNamespace"`

Usage:

 * -m = mode
      * c = create
      * d = deploy
 * -p = path to package to be created or to be deployed
 * -f = folder to be packaged or deployment folder
 * -t = token - can be delimited list "Token=\__Token\__" "Token1=\__Token1\__"
 
Packages are zip archives.
 
