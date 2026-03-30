# Sistem pentru Managementul Proiectelor de Cercetare - Backend

## Descriere

Acest proiect este o aplicație web ușor de folosit pentru gestiunea mai multor aspecte ale procesului de cercetare într-o universitate. Aceste aspecte includ:
1. Detaliile și obiectivele proiectului de cercetare;
2. Stagiile proiectului, care sunt alcătuite la rândul lor din mai multe aspecte:
    - Detalii cu privire la stagiu;
    - Rapoartele ce aparțin stagiului;
    - Rezultatele stagiului;
    - Bugetul stagiului;
    - Starea în care se află stagiul la momentul actual;
3. Membrii echipei de cercetare, precum și rolurile și sarcinile asociate fiecărui membru;
4. Achizițiile de resurse necesare proiectului;
5. Alocările de resurse aflate deja în cadrul universității.

## Detalii cu privire la backend

Backend-ul a fost implementat folosind C# și ASP .NET Core. Lucrul cu baza de date se realizează prin intermediul obiectelor .NET cu ajutorul ORM-ului [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/).

Sistemul de autorizare a fost implementat folosind tehnologia [JWT](https://www.jwt.io/) (JSON Web Token). Un utilizator poate accesa endpoint-urile protejate ale serverului doar dacă deține un token valid generat de backend în urma autentificării.

## Software Necesar
- [ASP .NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)

**Important:** Dacă serverul nu se poate conecta la baza de date, asigurați-vă că valoarea câmpului `ConnectionString` este validă.
