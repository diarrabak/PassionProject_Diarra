## PassionProject_Diarra
- This project is about the use of code-first and entity framework to create database with ASP .NET Framework
- An application allows kids living in remote area to have access to cheap and good quality education 

## Features
- There are 4 features and each feature has the full CRUD (Create, Read, Update and Delete) functionalities
   -  Courses
   -  Modules
   -  Pupils
   -  Location
- JS validation for all forms

## Feature tables and relationships
- The courses table is the main table and its primary key is used as foreign key in the other tables
- The modules table has a Many-to-One relationship with the course table
- The Pupils table has a Many-to-One relationship with the module table
- The location table has a Many-to-One relationship with the pupils table
## Challenges
- To realize with project, I had to learn from different resources such lectures notes, forum and GitHub
- Writting to the database is challenging at the first time since the communications between the controllers and the views do not work at the first attempt
