# Meme-of-the-Day Architecture Description

## C4 Model Architecture Description

### System Context Level

**Meme-of-the-Day** is a web application that provides users with a collection of entertaining content including memes, quotes, fun facts, and "would you rather" questions. The system is entirely self-contained with no external dependencies or integrations.

### Container Level

The system consists of two applications:

1. **Frontend Application (SvelteKit Application)**
   - **Technology**: SvelteKit framework with TypeScript
   - **Responsibility**: Provides the user interface, handles client-side interactions, and communicates with the backend API
   - **Components**: Layout, page components, and specialized content components

2. **Backend Application (.NET Core API)**
   - **Technology**: .NET Core Web API (version 9.0)
   - **Responsibility**: Provides data endpoints for the frontend, manages data models
   - **Components**: API controllers and data models

### Component Level

#### Frontend Components:

1. **App Layout (+layout.svelte)**
   - **Responsibility**: Provides the overall layout structure for the application

2. **Home Page (+page.svelte)**
   - **Responsibility**: Main page that houses all content components

3. **Content Components**:
   - **Meme Component (meme.svelte)**: Displays meme content
   - **Quote Component (quote.svelte)**: Displays quote content
   - **FunFact Component (funFact.svelte)**: Displays fun fact content
   - **WYR Component (wyr.svelte)**: Displays "would you rather" content

4. **Frontend API Routes**:
   - **fetchMeme (api/fetchMeme+server.ts)**: Fetches meme data from backend
   - **fetchQuote (api/fetchQuote/+server.ts)**: Fetches quote data from backend
   - **fetchFunFact (api/fetchFunFact/+server.ts)**: Fetches fun fact data from backend
   - **fetchWYR (api/fetchWYR/+server.ts)**: Fetches "would you rather" data from backend

#### Backend Components:

1. **API Controllers**:
   - **MemeController**: Handles GET requests for meme data
   - **QuoteController**: Handles GET requests for quote data
   - **FunfactController**: Handles GET requests for fun fact data
   - **WyrController**: Handles GET requests for "would you rather" data

2. **Data Models**:
   - **Meme Model**: Represents meme data (Id, ImageUrl, Title, Category)
   - **Quote Model**: Represents quote data (Id, Text, Author, Category)
   - **Funfact Model**: Represents fun fact data (Id, Text, Category)
   - **WouldYouRather Model**: Represents "would you rather" data (Id, OptionA, OptionB, Category)

### Code Level

#### Data Flow Pattern:

1. User interacts with the browser
2. Browser renders SvelteKit application
3. Application loads UI components
4. Components make fetch requests to SvelteKit server routes
5. Server routes forward HTTP requests to the backend API
6. API controllers process requests and access model data
7. Model data is returned to controllers, formatted as JSON
8. JSON responses flow back through the API to the frontend
9. Components update with the received data
10. Browser displays updated content to the user

#### Key Technical Details:

- **Static Assets**: Fonts (Muli-Regular.ttf, Oswald-Bold.ttf, Oswald-Regular.ttf) and a favicon
- **API Structure**: RESTful API endpoints with standard HTTP GET requests
- **Data Storage**: In-memory model instances (no database persistence)