// src/routes/api/fetchMeme/+server.js
export async function GET() {
    try {
        const response = await fetch("http://localhost:5000/api/fun-fact");
        
        if (!response.ok) {
            console.error("API error:", response.status, response.statusText);
            return new Response(JSON.stringify({ error: "Failed to fetch meme" }), {
                status: 500,
                headers: { 'Content-Type': 'application/json' }
            });
        }
        
        // The backend returns the URL as plain text
        const memeUrl = await response.text();

        return new Response(JSON.stringify({ url: memeUrl }), {
            headers: { 'Content-Type': 'application/json' }
        });
    } catch (error) {
        console.error("Error fetching meme:", error);
        return new Response(JSON.stringify({ error: "Failed to fetch meme" }), {
            status: 500,
            headers: { 'Content-Type': 'application/json' }
        });
    }
}