Templifier
==================================

Lightweight and portable tool than allows you to template a solution.
Based and inspired by https://github.com/endjin/Templify

From the command line you would create a package using the following command switches:

  Nlavri.Templifier.exe -m c -p C:\MyTemplate.pkg -f "c:\SampleSolutionFolder" -t "SampleBaseNamespace=__NAME__"

and you would deploy the package created above by using the following command switches:

  Nlavri.Templifier.exe -m d -p C:\MyTemplate.pkg -f "c:\SolutionBasedOnTemplateFolder" -t "__NAME__=NewBaseNamespace"

The switches have the following meaning:

 -m = mode
      c = create
      d = deploy
 -p = path to package to be created or to be deployed
 -f = folder to be packaged or deployment folder
 -t = token - can be delimited list "Token=__Token__" "Token1=__Token1__"
 
Packages are zip archives - renamed to .pkg

 
