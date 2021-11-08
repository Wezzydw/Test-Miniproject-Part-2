Feature: CreateBooking


@mytag
Scenario: Create hotel booking
	Given the start date is <StartDate>
	And the end date is <EndDate>
	When check if room is available
	Then the result should be <IsActive>

	Examples: 
	| StartDate | EndDate | IsActive |
	|	 21 	|	 25	  |   true   |
	|     1     |     9   |   true   |
	|     9     |    21   |   false  |
	|     9     |    10   |   false  |
	|     9     |    20   |   false  |
	|    21     |     9   |   false  |
	|    10     |     1   |   false  |
	|	 10		|	 11	  |	  false  |
	|	 10		|    20   |   false  |