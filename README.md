# LoggingProxyTest
## Running a docker container
	1. Navigate to solution root folder
	2. Run "docker build -t testtask_proxy ." in a cmd
	3. Run "docker run -p {hostport}:80 testtask_proxy" in a cmd
## Testing
	The solution has swagger page, available via the default link "/swagger/index.html"
## Possible improvements
	Due to the time constraints some things were skipped.
	For example:
	1. Paging on Get endpoint
	2. Exception handling is pretty primitive
	3. It could make sense to add the "Client" library/nuget package (swagger generated code)
	4. Unit test coverage is not perfect
	etc.
## Requirements no implemented
	1. Basic authorization (endpoints were left unprotected due to time constraints)
