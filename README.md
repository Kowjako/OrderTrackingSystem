# ⚓ UKTracker — rozbudowany system do zarządzania przesylkami
System śledzenia zamówień - moja praca dyplomowa, rozbudowany system do zarządzania zamówieniami, pozwala śledzić jak wysyłane tak i odbierane paczki pozwala budować wzorce reklamacyjne i dużo innych rzeczy.

# Wykorzystane narzędzia
🛡️ C# 8.0  
🛡️ Microsoft SQL Server  
🛡️ Entity Framework 6  
🛡️ ADO.NET  
🛡️ Windows Presentation Foundation (WPF) + Material Design  
🛡️ Fluent Validation  
🛡️ xUnit + Moq  

# Jak zainstalować:
1️⃣ Pobrać repo albo zrobić git clone   
2️⃣ Zbuildować projekt  
3️⃣ Odpalić Sql Server Managment Studio i wbić po kolei skrypty z folderu SQL:  
    👉 BaseDefinition  
    👉 ProcessDefinition  
    👉 TriggerDefinition  
4️⃣ Podmienić w całej solucji w każdym pliku App.config nazwę serwera z WLODEKPC\SQLEXPRESS na swoją.  
5️⃣ Wejść do Services/ConfigurationService zamienić również WLODEKPC\SQLEXPRESS na nazwę swojego SQL servera.  

# Możliwości:  
Strona klienta:
- Tworzenie zamówień, tworzenie wysyłek innym klientom, zarządzanie wiadomościami, tworzenie wzorców reklamacyjnych, tracking całego przebiegu zamówienia, korzystanie z bonów sklepu, itd.  
  
Strona sprzedawcy:
- Zarządzanie reklamacjami, kontrolowanie przebiegu przesyłek, zarządzanie korespondencją, generowanie bonów dla klientów, uruchamianie automatycznych procesów, itd.  

Ogólne możliwości:
- Aplikacja jest w pełni zlokalizowana w 3 językach (rosyjski, agnielski, polski).  

# Wygląd aplikacji
![Screenshot_1](https://user-images.githubusercontent.com/19534189/179368891-74c041ee-b52c-4dc8-8314-a05a7d980a23.jpg)
![Screenshot_2](https://user-images.githubusercontent.com/19534189/179368893-5d6a2bda-77db-4acf-9112-7cc40e102804.jpg)
![Screenshot_3](https://user-images.githubusercontent.com/19534189/179368896-6b4d2c62-d8f1-4138-8c67-b466dc117ced.jpg)
![Screenshot_4](https://user-images.githubusercontent.com/19534189/179368897-b38a1eae-4d81-4631-9e70-36eeb0e01f35.jpg)
![Screenshot_5](https://user-images.githubusercontent.com/19534189/179368898-f91d2781-c8b5-4be8-9c8f-1151b5c218a4.jpg)
![Screenshot_6](https://user-images.githubusercontent.com/19534189/179368900-a07a906e-d672-4587-adc5-619e7983fc3b.jpg)
![Screenshot_7](https://user-images.githubusercontent.com/19534189/179368903-a70dda84-1813-42b2-a4da-b7d08feaf436.jpg)
![Screenshot_8](https://user-images.githubusercontent.com/19534189/179368904-82405676-7979-446c-bed0-d5c6da333334.jpg)
![Screenshot_9](https://user-images.githubusercontent.com/19534189/179368905-e743d109-3cbf-4565-af48-daa0c7f03822.jpg)
