# Lab 7 - Autentificare si Autorizare in ASP.NET Core MVC
### Demo video :
https://youtube.com/shorts/2lumT6aAauU?is=dtTWwssp8vADhEvQ


Aici am rezolvat exercitiile pentru implementarea securitatii pe articole:

**Ex 1 - Setare automata a autorului la creare:**
- Am sters dropdown-ul prin care se selecta manual autorul articolului din formularul de "Create".
- Am inlocuit logica in `ArticlesController` (metoda POST) astfel incat `UserId` sa fie preluat automat din `User.Identity.Name` (email-ul contului logat).

**Ex 2 - Content Ownership pentru Edit si Delete:**
- Am adaugat metoda ajutatoare `IsOwnerOrAdmin(article, cancellationToken)` in `ArticlesController`.
- Am pus protectie suplimentara in actiunile GET si POST pentru Edit si Delete. Daca utilizatorul nu este proprietarul articolului vizat sau nu are rolul de "Admin", actiunea returneaza `Forbid()`.

**Ex 3 - Cleanup si UX:**
- In View-uri (`Index.cshtml` si `Details.cshtml`), am adaugat o verificare la afisarea butoanelor de "Editare" si "Stergere", astfel incat acestea sa apara doar daca utilizatorul este logat si este fie autorul articolului, fie Admin.
- In `Edit.cshtml`, am scos campul de selectie pentru autor pentru a bloca modificarea acestuia.
- Am sters modelul, repository-ul si serviciul `User` personalizat din aplicatie, inlocuindu-le corespunzator in cod ca aplicatia sa se bazeze pe sistemul nativ `User.Identity` din ASP.NET Core.

---

### Raspunsuri Teoretice:

**1. De ce Logout este implementat ca form method="post" si nu ca un simplu link href?**

Logout-ul modifica starea aplicatiei (sesiunea este distrusa). Daca ar fi un link GET, ar exista riscul ca browserele (care uneori preincarca link-urile paginii) sa deconecteze utilizatorul automat fara ca acesta sa dea click. De asemenea, request-urile GET sunt vulnerabile la atacuri CSRF, unde un site extern ar putea forta delogarea. Folosind un POST restrictiv se asigura ca actiunea se executa corect si sigur.

**2. De ce login-ul se face in 2 pasi (gasim userul dupa email, apoi logam cu UserName)?**

In arhitectura ASP.NET Core Identity, metoda de autentificare `PasswordSignInAsync` este construita sa primeasca drept prim argument `UserName`-ul, nu `Email`-ul. Cum in aplicatia noastra dorim ca utilizatorul sa se conecteze cu adresa de email, trebuie sa facem intai interogarea bazei de date cu `FindByEmailAsync` pentru a extrage proprietatea `UserName` asociata acelui cont, pe care o pasam apoi metodei interne de logare.

**3. De ce protejam atat in controller cu [Authorize]+IsOwner, cat si in View cand ascundem butoanele?**

Ascunderea butoanelor in UI e doar pentru experienta utilizatorului (sa nu vada actiuni pe care nu le poate face). Insa nu ofera niciun fel de securitate in sine; un utilizator poate tasta direct in bara de adrese URL-ul care duce catre `/Articles/Edit/5` sau poate folosi unelte gen Postman. Prin urmare, protectia reala trebuie implementata obligatoriu direct in Controller-ul backend unde se face si verificarea finala a drepturilor. Daca puneam restrictia doar in backend, utilizatorii ar vedea si apasa pe acele butoane doar ca sa primeasca o eroare, ceea ce inseamna o interfata neclara.

**4. Ce este middleware pipeline? De ce UseAuthentication inainte de UseAuthorization?**

Pipeline-ul middleware reprezinta tot fluxul secvential prin care trece o cerere HTTP de cand intra in aplicatie pana ia un raspuns. Componentele trebuie inlanțuite corect din punct de vedere logic. Metoda `UseAuthentication()` identifica utilizatorul pe baza token/cookie-ului oferit. Metoda `UseAuthorization()` vine la randul ei si aproba sau respinge accesul acelei persoane catre cererea ei in functie de regulile aplicatiei. Cele doua trebuie sa stea in aceasta ordine pentru ca e imposibil teoretic sa dai cuiva drepturi sau interdictii atat timp cat tu inca n-ai procesat anterior ca sa afli cine e, sau daca macar s-a logat.

**5. Ce face ASP.NET Core Identity in locul nostru manual?**

Fara Identity ar fi trebuit sa inventam personal sisteme de la zero pentru inregistrarea utilizatorului vizual cat si baza de date, metode sigure pentru hashing al parcelelor, persistenta logarii (generarea si validarea unui Cookie securizat care mentine sesiunea valida), structura in care se impart grupurile si rolurile pe diferite clase de user, ba chiar si procese lungi teoretice (ca validarea email-ului sau recuperarea parcelei). Pachetul Identity ne ofera gata-facute toata aceasta arhitectura si asigura in spate un standard inalt de securitate.

**6. Dezavantaje ASP.NET Core Identity?**

Pe termen lung vine cu anumite limitari de control. Pachetul se instaleaza fixat cu propria schematica de model pe baza de date unde adauga multe tabele extra care uneori ne sunt inutile. De asemenea, cu Identity e mai complicat sa integrezi aplicatii si clientii decuplati (ceea ce se cheama SPA frontend - Angular/React - sau aplicatii Mobile) unde tipul de mentinere a logarii prin Cookie generat de Identity nu ruleaza optim. Marea lui solutie ar fi pe proiecte statice precum acest MVC.
