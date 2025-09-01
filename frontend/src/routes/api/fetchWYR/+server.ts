// src/routes/api/fetchWYR/+server.ts
export async function GET() {
    try {
        const response = await fetch("http://localhost:5000/api/wyr");
        
        if (!response.ok) {
            console.error("API error:", response.status, response.statusText);
            return new Response(JSON.stringify({ error: "Failed to fetch WYR" }), {
                status: 500,
                headers: { 'Content-Type': 'application/json' }
            });
        }

        const wyr = await response.json();
        
        // If you need to split the question by "or", do it here with the actual question string
        if (wyr && typeof wyr.question === 'string') {
            let keyword = " or ";
            let parts = wyr.question.split(keyword);

            // Assign to variables
            let before: string = parts[0];
            let after: string = parts[1];

            keyword = "Would you rather ";
            const afterParts: string[] = before.split(keyword);

            before = afterParts[1]
            // If you need to return these parts, add them to the response
            return new Response(JSON.stringify({
                before,
                after,
            }), {
                headers: { 'Content-Type': 'application/json' }
            });
        }

        return new Response(JSON.stringify(wyr), {
            headers: { 'Content-Type': 'application/json' }
        });
    } catch (error) {
        console.error("Error fetching wyr:", error);
        return new Response(JSON.stringify({ error: "Failed to fetch WYR" }), {
            status: 500,
            headers: { 'Content-Type': 'application/json' }
        });
    }
}