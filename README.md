# LogFileReader
A tool used for reading and getting insights from log files.

## Features
- Supports log files that are written in the Apache CLF format. Reports on all invalid properties or log line formats before exiting the program.
- Can retrieve the number of unique IP's from a log file.
- Can retrieve the top 3 most visited URL's from the log file (if there are url's that are tied, the latest ones are retrieved.).
- Can retrieve the top 3 most active IP's from the log file (if there are url's that are tied, the latest ones are retrieved.).

## Prerequisites
- Use .net 8 runtime

## Installation
To install the project, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/thomson-jayden/LogFileReader.git
    ```
2. Navigate to the project directory:
    ```bash
    cd LogFileReader
    ```
3. Restore the dependencies:
    ```bash
    dotnet restore
    ```

## Usage
To run the project, execute the following command:
```bash
LogFileReaderConsoleApp <filepath>
```

## Configuration
No configuration required.

## Running Tests
To run the test suite, execute the following command:
```bash
dotnet test
```

## Deployment
No deployment pipeline currently implemented.

## Future Considerations
- Currently implemented as a console app, however it is possible to reuse the code found in th library project
to be in an API or a Batch Job if requirements evolve.
- Change console app to have a secondary argument that changes the `top` variable.