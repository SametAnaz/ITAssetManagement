# View Categories

## Shared Views

### _Layout.cshtml
The main layout template that provides the common structure for all pages.

#### Features
- Responsive navigation bar
- Sidebar menu
- Footer
- Common CSS/JS includes
- User session information
- Notification area

#### Sections
- Title
- Styles
- Scripts
- Content

#### ViewData Properties
- Title
- ActiveMenu
- UserName
- Notifications

### _ValidationScriptsPartial.cshtml
Partial view for client-side validation scripts.

#### Includes
- jQuery Validation
- jQuery Unobtrusive Validation
- Custom validation rules

## Assignment Views

### Index.cshtml
Displays a list of all assignments with filtering and sorting.

#### Features
- Paginated list
- Search functionality
- Sort by columns
- Quick actions
- Status indicators

#### ViewData
- CurrentSort
- SearchString
- PageSize

### Create.cshtml
Form for creating new assignments.

#### Features
- User selection with search
- Laptop selection
- Date range picker
- QR code generation
- File attachments

#### JavaScript Functions
- generateQRCode()
- validateDates()
- searchUsers()
- searchLaptops()

[View more details about Assignment views](assignments.md)

## Laptop Views

### Index.cshtml
Displays list of all laptops with inventory management features.

#### Features
- Stock level tracking
- Search and filter
- Quick actions
- Status indicators

### DeletedLaptops.cshtml
Shows soft-deleted laptops with restore options.

#### Features
- Restore functionality
- Deletion history
- Filter options

[View more details about Laptop views](laptops.md)

## User Views

### Index.cshtml
Lists all users with role and assignment information.

#### Features
- Role indicators
- Active assignments
- Search functionality
- Quick actions

### Create.cshtml
User creation form with role management.

#### Features
- Basic info fields
- Role selection
- Department assignment
- Profile photo upload

[View more details about User views](users.md)

## Authentication Views

### Login.cshtml
User authentication view.

#### Features
- Username/password form
- Remember me option
- Validation
- Error messages
- Password reset link

## Common View Patterns

### Form Handling
```cshtml
@model CreateAssignmentViewModel

<form asp-action="Create" method="post" enctype="multipart/form-data">
    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
    
    <div class="form-group">
        <label asp-for="UserId"></label>
        <select asp-for="UserId" asp-items="@Model.Users" class="form-control">
            <option value="">Select User</option>
        </select>
        <span asp-validation-for="UserId" class="text-danger"></span>
    </div>
    
    <!-- Other form fields -->
</form>
```

### Table Listings
```cshtml
<table class="table table-hover">
    <thead>
        <tr>
            <th>
                <a asp-action="Index" asp-route-sortOrder="@ViewData["NameSort"]">
                    @Html.DisplayNameFor(model => model.Name)
                </a>
            </th>
            <!-- Other headers -->
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model) {
            <tr>
                <td>@Html.DisplayFor(modelItem => item.Name)</td>
                <!-- Other cells -->
            </tr>
        }
    </tbody>
</table>
```

### Partial Views
```cshtml
@await Html.PartialAsync("_StatusPartial", Model.Status)
```

## View Guidelines

### Naming Conventions
- PascalCase for view names
- Prefix partial views with underscore
- Match controller action names
- Use descriptive suffixes (Create, Edit, Delete)

### Code Organization
- Use tag helpers over HTML helpers
- Keep business logic in controllers
- Use view components for complex UI parts
- Follow single responsibility principle

### Performance
- Enable bundling and minification
- Use async operations where possible
- Implement lazy loading
- Cache partial views

### Security
- Always use model binding
- Implement anti-forgery tokens
- Validate all user input
- Encode output properly

### Accessibility
- Use semantic HTML
- Include ARIA attributes
- Maintain keyboard navigation
- Follow WCAG guidelines
