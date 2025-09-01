// src/routes/api/fetchFunFact/+server.ts
export async function GET() {
    try {
        const response = await fetch("http://localhost:5000/api/fun-fact");
        
        if (!response.ok) {
            console.error("API error:", response.status, response.statusText);
            return new Response(JSON.stringify({ error: "Failed to fetch funfact" }), {
                status: 500,
                headers: { 'Content-Type': 'application/json' }
            });
        }
        
        const funfact = await response.json();

        return new Response(JSON.stringify(funfact), {
            headers: { 'Content-Type': 'application/json' }
        });
    } catch (error) {
        console.error("Error fetching funfact:", error);
        return new Response(JSON.stringify({ error: "Failed to fetch funfact" }), {
            status: 500,
            headers: { 'Content-Type': 'application/json' }
        });
    }
}