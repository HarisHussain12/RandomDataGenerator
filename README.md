# Random Data Processor System

## 📌 Project Description
A two-part .NET solution that:
1. **Generates** large files containing random data (Challenge A)
2. **Processes** these files with different output modes (Challenge B)

**Key Features:**
- Generates 10MB files with 4 data types:
  - Alphabetical strings
  - Real numbers
  - Integers
  - Alphanumerics with random spacing
- Processes files differently based on execution environment:
  - **Local mode**: Prints parsed objects to console
  - **Docker mode**: Saves processed results to file
- Container-ready with Docker support

### Project Structure
 RandomDataGenerator/  
 ├── RandomDataGenerator/ # Generator project (Challenge A)  
 ├── RandomDataReader/ # Processor project (Challenge B)  
 ├── Data/ # Shared data volume  
 │     ├── Input/ # For Challenge A output  
 │     └── Output/ # For Challenge C output  
 ├── docker-compose.yml # Container orchestration  
 └── README.md # This file  

### Prerequisites
- .NET 8.0 SDK  
- Docker Desktop (for containerization)  
- Git (optional)  

## 1. RandomDataGenerator (Challenge A)
Creates 10MB files containing randomized:  
  -Alphabetical strings (e.g., HelloWorld) 
  -Real numbers (e.g., 3.14159)  
  -Integers (e.g., -42)  
  -Alphanumerics with random 0-10 spaces (e.g., AB12 )  

### Key Features:
**✔ Configurable output size via appsettings.json**  
**✔ CSV-style comma-separated formatting**

### Output Example:
###### ABC123,3.14,-42, Hello ,9876,2.71828,...

### Output File Path:
###### RandomDataReader/Data/Input (For Challenge B)

### Configuration
~File size (MB)  
~Output path   
~Data generation rules  

## 2. RandomDataReader (Challenge B)
Reads Challenge A's generated files and:  
  -Identifies all 4 data types (strings, integers, reals, alphanumerics)  
  -Trims spaces from alphanumerics  
  -Outputs to console (local) or file (Docker)  

### Key Features:
#### ✔ Dual-mode operation:
**Local**: Prints results to console  
**Docker**: Saves space-trimmed output to /Data/Output/

### Step To Run:
1- First run build command on **RandomDataReader** folder.  
  `docker build -t data-processor -f Dockerfile`  

2- Run docker    
  `docker-compose up`  

### Output Example:
###### ABC123  → Alphanumeric  
###### 42      → Integer
###### 3.14    → Real Number  
###### Hello   → Alphabetical  

### Output File Path:
###### RandomDataReader/Data/Output

### Configuration
~Input File Path  
~Output File Path  
