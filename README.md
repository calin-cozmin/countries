# 🌍 Countries API Service

## 📌 Description

This is a RESTful API service that provides detailed information about countries. It fetches data from an external API and provides enhanced functionalities such as:
- 📝 Filtering by country name
- 📊 Limiting results by population
- 🔤 Sorting countries by name
- 📄 Pagination

---

## 🛠️ How to Run Locally

### Prerequisites

- **.NET 5.0 SDK** or higher
- **Visual Studio 2019** (or any compatible IDE)

### 🚀 Steps

1. **Clone the Repository**
    ```bash
    git clone https://github.com/your-repository/countries-api-service.git
    ```
   
2. **Navigate to the Project Folder**
    ```bash
    cd countries-api-service
    ```

3. **Restore Dependencies**
    ```bash
    dotnet restore
    ```

4. **Run the Application**
    ```bash
    dotnet run --project ./countries/countries.csproj
    ```

5. **Browse**
    Navigate to `http://localhost:5000` or `https://localhost:5001` in your web browser.

---

## 🎯 Endpoint Examples

🌐 **Base URL**: `http://localhost:5000/api/countries`

1. **Get All Countries**
    ```http
    GET /api/countries
    ```

2. **Filter Countries by Name ('st')**
    ```http
    GET /api/countries?countryName=st
    ```

3. **Limit by Population (2 Million)**
    ```http
    GET /api/countries?maxPopulation=2
    ```

4. **Sort by Name (Ascending)**
    ```http
    GET /api/countries?sort=ascend
    ```

5. **Sort by Name (Descending)**
    ```http
    GET /api/countries?sort=descend
    ```

6. **Combine Name Filter and Population**
    ```http
    GET /api/countries?countryName=st&maxPopulation=2
    ```

7. **Combine Name Filter and Sorting (Ascending)**
    ```http
    GET /api/countries?countryName=st&sort=ascend
    ```

8. **Limit Results (Top 5 Countries)**
    ```http
    GET /api/countries?take=5
    ```

9. **Combine All Filters and Limit**
    ```http
    GET /api/countries?countryName=st&maxPopulation=2&take=3
    ```

10. **Combine All Filters, Sorting (Descending) and Limit**
    ```http
    GET /api/countries?countryName=st&sort=descend&take=3
    ```

---
