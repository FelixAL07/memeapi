// src/routes/api/fetchQuote/+server.ts
export async function GET() {
    try {
        const response = await fetch("http://localhost:5000/api/quote");
        
        if (!response.ok) {
            console.error("API error:", response.status, response.statusText);
            return new Response(JSON.stringify({ error: "Failed to fetch quote" }), {
                status: 500,
                headers: { 'Content-Type': 'application/json' }
            });
        }
        
        // The backend returns an array of quotes from the API Ninjas service
        const quoteData = await response.json();
        
        if (quoteData != null) {
            
            return new Response(JSON.stringify(quoteData[0]), {
                headers: { 'Content-Type': 'application/json' }
            });
        } else {
            return new Response(JSON.stringify({ error: "Invalid quote format received" }), {
                status: 500,
                headers: { 'Content-Type': 'application/json' }
            });
        }
    } catch (error) {
        console.error("Error fetching quote:", error);
        return new Response(JSON.stringify({ error: "Failed to fetch quote" }), {
            status: 500,
            headers: { 'Content-Type': 'application/json' }
        });
    }
}