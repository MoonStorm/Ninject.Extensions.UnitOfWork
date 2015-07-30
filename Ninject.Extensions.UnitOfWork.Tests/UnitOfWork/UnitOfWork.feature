Feature: Test IOC capabilities in a non-web environment 

Scenario: Multiple resolves retrieve the same service instance under a single unit-of-work
	Given I have set up the application IOC container
	When I register the sample IOC service with an application unit-of-work binding
	And I start a new application unit of work
	And I resolve the sample service through the application IOC container
	And I resolve the sample service through the application IOC container
	Then the sample services 0,1 resolved through the application IOC container should be the same

Scenario: Nested unit-of-work contexts
	Given I have set up the application IOC container
	When I register the sample IOC service with an application unit-of-work binding
	# Start Unit of Work #1
	And I start a new application unit of work
	#      Resolve service #0
	And I resolve the sample service through the application IOC container
	And I end the current application unit of work
	# End Unit of Work #1
	# Start Unit of Work #2
	And I start a new application unit of work
	#      Resolve service #1
	And I resolve the sample service through the application IOC container
	# Start Unit of Work #3 - inner
	And I start a new application unit of work
	#      Resolve service #2
	And I resolve the sample service through the application IOC container
	And I end the current application unit of work
	# End Unit of Work #3 - inner
	#      Resolve service #3
	And I resolve the sample service through the application IOC container
	# End Unit of Work #2
	# Note that we haven't ended unit of work #1

	Then the sample services 0,1,2 resolved through the application IOC container should not be the same
	And the sample services 1,3 resolved through the application IOC container should be the same
	And the sample services 0,2 resolved through the application IOC container should be disposed
	And the sample services 1,3 resolved through the application IOC container should not be disposed

Scenario: The unit-of-work context is passed onto new tasks
	Given I have set up the application IOC container
	When I register the sample IOC service with an application unit-of-work binding
	And I start a new application unit of work
	And I resolve the sample service through the application IOC container
	And I resolve the sample service in a new task through the application IOC container
	And I end the current application unit of work
	Then the sample services 0,1 resolved through the application IOC container should be the same
	And the sample services 0,1 resolved through the application IOC container should be disposed

Scenario: The unit-of-work context is passed across async calls
	Given I have set up the application IOC container
	When I register the sample IOC service with an application unit-of-work binding
	And I start a new application unit of work
	And I resolve the sample service through the application IOC container
	And I resolve the sample service async through the application IOC container
	And I end the current application unit of work
	Then the sample services 0,1 resolved through the application IOC container should be the same
	And the sample services 0,1 resolved through the application IOC container should be disposed


