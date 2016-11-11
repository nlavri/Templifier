Templifier
==================================

Lightweight and portable tool that allows you to template a solution.

Based on and inspired by https://github.com/endjin/Templify 
but doesn't require installation and use nuget-only 3-rd party referenes.

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
 
