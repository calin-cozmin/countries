Countries API Service
Description
This is a RESTful API service for fetching country information. It fetches data from an external API and provides additional functionalities like filtering by country name, maximum population, sorting by country name, and pagination.

How to Run Locally
Prerequisites
.NET 5.0 SDK or higher
Visual Studio 2019 (or equivalent IDE)
Steps
Clone the repository:
bash
Copy code
git clone https://github.com/your-repository/countries-api-service.git
Navigate to the project folder:
bash
Copy code
cd countries-api-service
Restore the dependencies:
bash
Copy code
dotnet restore
Run the application:
bash
Copy code
dotnet run --project ./countries/countries.csproj
Navigate to http://localhost:5000 or https://localhost:5001 in your browser.
Endpoint Examples
Base URL: http://localhost:5000/api/countries

1. Get all countries
http
Copy code
GET /api/countries
2. Get countries with name containing "st"
http
Copy code
GET /api/countries?countryName=st
3. Get countries with maximum population of 2 million
http
Copy code
GET /api/countries?maxPopulation=2
4. Sort countries in ascending order by name
http
Copy code
GET /api/countries?sort=ascend
5. Sort countries in descending order by name
http
Copy code
GET /api/countries?sort=descend
6. Get countries with name containing "st" and maximum population of 2 million
http
Copy code
GET /api/countries?countryName=st&maxPopulation=2
7. Get countries with name containing "st" and sort them in ascending order
http
Copy code
GET /api/countries?countryName=st&sort=ascend
8. Limit the number of returned countries to 5
http
Copy code
GET /api/countries?take=5
9. Get countries with name containing "st", maximum population of 2 million, and limit the results to 3
http
Copy code
GET /api/countries?countryName=st&maxPopulation=2&take=3
10. Get countries with name containing "st", sorted in descending order, and limit the results to 3
http
Copy code
GET /api/countries?countryName=st&sort=descend&take=3