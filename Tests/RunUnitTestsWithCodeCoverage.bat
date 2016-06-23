REM Use local version of OpenCover if environment variable is not set
if ["%OPEN_COVER_HOME%"]==[""] (
  echo OPEN_COVER_HOME variable not set, using local version.
  set OPEN_COVER_HOME=Ext\OpenCover\
)

cd ..
"%OPEN_COVER_HOME%OpenCover.Console.exe" -target:"Ext\NUnit\bin\nunit3-console.exe" -targetargs:"Tests\UnitTests.nunit --result=UnitTestsResult.xml;format=nunit2" -output:"Tests/CodeCoverageResult.xml" -coverbytest:*.Tests.dll -register:user
Ext\ReportGenerator\bin\ReportGenerator.exe -reports:"Tests/CodeCoverageResult.xml" -targetdir:"Tests/CodeCoverageReport"