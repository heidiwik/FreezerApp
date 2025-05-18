# FreezerApp

A Blazor Web App for managing freezer inventory, using Azure Table Storage as the backend.  
Easily add, view, group, and delete items stored in your freezer, with support for multiple compartments and box numbers.

---

## Features

- **Add Items:**  
  Add new freezer items, specifying name, quantity, one or more box numbers, compartment, and storage date.

- **View Items:**  
  See all items in a grouped table (grouped by name), with total quantity, box numbers, compartments, and earliest storage date.

- **Delete Items:**  
  Delete individual items or all items with the same name at once.

- **Azure Table Storage Integration:**  
  All data is stored in Azure Table Storage for scalability and reliability.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- An [Azure Storage Account](https://portal.azure.com/)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Configuration

1. **Clone the repository:**
    ```
    git clone <your-repo-url>
    cd FreezerApp
    ```
2. **Set up Azure Table Storage:**
    - Create a Storage Account in Azure.
    - Create a Table named `FreezerItems`

3. **Configure connection settings:**
    - In your appsettings.Development.json (or user secrets), add:
    ```
    "Storage": {
      "ConnectionString": "<your-connection-string>",
      "AccountUri": "<your-storage-account-uri>"
    }
    ```
  - The app uses the connection string when debugging, and `DefaultAzureCredential` with `AccountUri` in production.

### Running the App
```dotnet run```

- Open your browser to [https://localhost:5001](https://localhost:5001) (or the URL shown in the console).

---

## Project Structure

- `FreezerApp/Components/Pages/`
  - `Home.razor` — Main page, displays grouped items.
  - `AddItem.razor` — Form to add new items.
  - `Error.razor` — Error page.
- `FreezerApp/Models/FreezerItem.cs` — Data model for freezer items.
- `FreezerApp/Services/TableService.cs` — Handles Azure Table Storage operations.

---

## Customization

- **Compartments:**  
  The app supports three predefined freezer compartments: "Compartment 1", "Compartment 2", "Compartment 3".  
  You can change these in `AddItem.razor` to suit your needs.

- **Box Numbers:**  
  Enter multiple box numbers separated by commas when adding an item. Each box number creates a separate row.

---

## Troubleshooting

- **Azure Authentication:**  
  - In development, ensure your connection string is correct.
  - In production, the app uses `DefaultAzureCredential` (managed identity, environment variables, etc.).

- **Validation:**  
  - Item name is required. The form will show an error if left blank.

---

## License

This project is provided as-is for educational and demonstration purposes.