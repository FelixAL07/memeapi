# Meme of the Day - Technical Documentation

## 1. Architecture and Structure of the Solution

### 1.1 Overall Architecture

The "Meme of the Day" application follows a client-server architecture with two main components:

1. **Frontend Application**
   - **Technology**: SvelteKit with TypeScript
   - **Purpose**: Provides the user interface and client-side logic
   - **Runs on**: Port 5173 (development mode)

2. **Backend Application**
   - **Technology**: .NET Core API (version 9.0)
   - **Purpose**: Serves as the API layer and handles data fetching from external sources
   - **Runs on**: Port 5000 (HTTP), Port 5001 (HTTPS)

### 1.2 Solution Structure

#### Backend (.NET Core API)

```
api/
├── Controllers/                # API endpoint controllers
│   ├── FunfactController.cs    # Handles fun fact requests
│   ├── MemeController.cs       # Handles meme requests
│   ├── QuoteController.cs      # Handles quote requests
│   └── WyrController.cs        # Handles "would you rather" requests
├── Models/                     # Data models
│   ├── Funfact.cs              # Fun fact data structure
│   ├── Meme.cs                 # Meme data structure
│   ├── Quote.cs                # Quote data structure
│   └── WouldYouRather.cs       # "Would you rather" data structure
├── Program.cs                  # Application entry point and configuration
├── appsettings.json            # App configuration settings
└── api.csproj                  # Project file with dependencies
```

#### Frontend (SvelteKit)

```
frontend/
├── src/
│   ├── lib/                    # Shared components and utilities
│   │   ├── components/         # Reusable UI components
│   │   │   ├── funFact.svelte  # Fun fact component
│   │   │   ├── meme.svelte     # Meme component
│   │   │   ├── quote.svelte    # Quote component
│   │   │   └── wyr.svelte      # "Would you rather" component
│   │   ├── assets/             # Static assets
│   │   └── index.ts            # Entry point for lib exports
│   ├── routes/                 # Application routes (pages)
│   │   ├── +layout.svelte      # Main application layout
│   │   ├── +page.svelte        # Main page that displays all components
│   │   └── api/                # Server-side API routes
│   │       ├── fetchFunFact/   # Fun fact API endpoint
│   │       ├── fetchMeme/      # Meme API endpoint 
│   │       ├── fetchQuote/     # Quote API endpoint
│   │       └── fetchWYR/       # "Would you rather" API endpoint
│   ├── app.html                # HTML template
│   └── app.d.ts                # TypeScript declarations
├── static/                     # Static assets (fonts, favicon, etc.)
├── package.json                # Package configuration and dependencies
└── svelte.config.js            # Svelte configuration
```

## 2. Communication between Backend and Frontend

### 2.1 Communication Flow

The application follows a typical client-server communication pattern:

1. **Frontend-to-Backend Communication**:
   - Frontend components initiate HTTP requests to SvelteKit server endpoints
   - SvelteKit server endpoints forward these requests to the .NET Core backend API
   - The backend processes requests and returns data
   - The frontend components display the received data

2. **API Endpoints**:
   - The backend exposes several REST API endpoints:
     - `/api/meme-of-the-day` - Returns a URL to a unique meme image
     - `/api/fun-fact` - Returns a random fun fact
     - `/api/quote` - Returns a quote with author and category
     - `/api/wyr` - Returns a "would you rather" question

3. **Cross-Origin Resource Sharing (CORS)**:
   - The backend is configured with CORS to allow requests from the frontend origin (`http://localhost:5173`)
   - This ensures secure communication between the separate frontend and backend servers

### 2.2 Data Flow Diagram

```
┌─────────────┐     HTTP Request     ┌─────────────────┐     HTTP Request     ┌─────────────┐
│  Frontend   │ ────────────────────►│ SvelteKit       │ ────────────────────►│   Backend   │
│ (Browser)   │                      │ Server Routes   │                      │ (.NET API)  │
└─────────────┘     HTTP Response    └─────────────────┘     HTTP Response    └─────────────┘
      ▲                                                                              │
      │                                                                              │
      │                                                                              ▼
      │                                                                      ┌─────────────┐
      └──────────────────────────────────────────────────────────────────────┤ External APIs│
                                     Data Flow                                └─────────────┘
```

## 3. External APIs Used

The application interacts with several external APIs to fetch different types of content:

### 3.1 Meme API
- **Provider**: Meme API (https://meme-api.com/gimme)
- **Usage**: Fetches random meme content including image URL, title, and source
- **Authentication**: None required
- **Implementation**: `MemeController.cs` fetches and processes meme data
- **Features**: 
  - Includes image processing to avoid duplicate memes using perceptual hashing
  - Uses SixLabors.ImageSharp and CoenM.ImageSharp.ImageHash libraries

### 3.2 Fun Facts API
- **Provider**: Useless Facts API (https://uselessfacts.jsph.pl/api/v2/facts/random)
- **Usage**: Retrieves random fun facts
- **Authentication**: None required
- **Implementation**: `FunfactController.cs` handles fetching fun facts

### 3.3 Quotes API
- **Provider**: API Ninjas (https://api.api-ninjas.com/v1/quotes)
- **Usage**: Fetches inspirational or funny quotes with author attribution
- **Authentication**: API key required (stored in environment variable `NINJA_API_KEY`)
- **Implementation**: `QuoteController.cs` manages quote retrieval

### 3.4 Would You Rather API
- **Provider**: Would You Rather API (https://would-you-rather.p.rapidapi.com/wyr/random)
- **Usage**: Retrieves "would you rather" scenario questions
- **Authentication**: RapidAPI key required (stored in environment variable `RAPDID_API_WYR_KEY`)
- **Implementation**: `WyrController.cs` handles fetching and caching WYR questions

## 4. Logic for "Meme of the Day" and Rotating Content

### 4.1 Meme of the Day Logic

The Meme of the Day implementation uses a sophisticated approach to ensure uniqueness and prevent duplicate memes:

1. **Uniqueness Algorithm**:
   - The backend fetches random memes from the external API
   - Each image is downloaded and processed to create a perceptual hash using CoenM.ImageSharp.ImageHash
   - This hash is compared against previously seen memes using a similarity threshold (>95%)
   - If the image is too similar to a previous one, another meme is fetched
   - This continues until a unique meme is found or the maximum attempt limit is reached (500 attempts)

2. **Storage**:
   - Unique meme hashes are stored in a dictionary (`hashDict`) to track seen memes
   - The URLs are stored in an array (`urlArr`) for reference
   - This in-memory storage persists for the lifetime of the application instance

3. **Frontend Implementation**:
   - The frontend component (`meme.svelte`) is configured to auto-refresh content
   - Default refresh interval is 1 hour (360,000 milliseconds)
   - Can be customized through component props (`autoRefresh` and `refreshInterval`)

### 4.2 Fun Facts and Other Content Rotation

1. **Would You Rather Content**:
   - Implementation includes a daily rotation mechanism
   - Content is cached with a date timestamp
   - New content is only fetched when the cached item is from a previous day
   - This ensures the "would you rather" question changes only once per day

2. **Fun Facts and Quotes**:
   - These are fetched fresh on each request without caching
   - The frontend components can be configured to auto-refresh
   - Multiple instances of the `FunFact` component can be used on the page

## 5. Bonus Features and Improvements

### 5.1 Content Deduplication

- The meme service uses perceptual image hashing to detect and prevent duplicate memes
- This ensures users always see fresh content even across application restarts
- The implementation can identify similar images even if they have slight variations

### 5.2 Environment Configuration

- The application uses dotenv.net to load environment variables from .env files
- This keeps sensitive API keys out of source control
- Supports different configurations for development and production environments

### 5.3 Error Handling and Resilience

- Comprehensive error handling in both frontend and backend
- Frontend components display loading states during data fetching
- Error messages are properly captured and displayed to users
- Backend includes retry logic for external API failures

### 5.4 Component Architecture

- The frontend uses a modular component approach
- Each content type has its own dedicated component
- Components can be reused and configured with props
- This architecture supports easy addition of new content types

### 5.5 Performance Optimizations

- Content caching for the "Would You Rather" feature reduces API calls
- Image processing only occurs when necessary
- The backend is configured for efficient cross-origin resource sharing

### 5.6 Design and UI

- Custom fonts (Muli and Oswald) for improved typography
- Responsive layout that works on different screen sizes
- Clean card-based design separates different content types

## 6. Getting Started

### 6.1 Backend Setup

1. Navigate to the api directory:
   ```bash
   cd api
   ```

2. Create a `.env` file with the required API keys:
   ```
   NINJA_API_KEY=your_api_ninjas_key
   RAPDID_API_WYR_KEY=your_rapidapi_key
   ```

3. Run the backend:
   ```bash
   dotnet run
   ```

### 6.2 Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   pnpm install
   ```

3. Run the development server:
   ```bash
   pnpm run dev
   ```

4. Access the application at `http://localhost:5173`
