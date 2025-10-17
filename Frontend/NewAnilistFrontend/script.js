console.log("Loaded script:", document.currentScript.src);

var url = "http://localhost:5243/Show/get-shows";


let currentPage = 1;
async function  handleOnClick(){

currentPage++
fetchJson();

}

document.getElementById("LoadButton").addEventListener("click",handleOnClick);


async function fetchJson() {
    try {
        const response = await fetch(url ,{
method: "POST",
headers: {"Content-Type": "application/json"},
body: JSON.stringify({ currentPage: currentPage })

});       
        const json = await response.json();       
        const outputDiv = document.getElementById("output");
        console.log(json);                        

        json.forEach(anime => {
            const animeDiv = document.createElement("div");
            animeDiv.innerHTML = `
                <a href="../details.html?id=${anime.id}">
                <h3>${anime.title.english ?? anime.title.romaji} </h3>   
                <img src="${anime.coverImage.large}" alt="${anime.title.romaji}">
                </a>
            `;
            outputDiv.appendChild(animeDiv);
        });

    } catch (error) {
        console.error('error:', error);           
    }
}



fetchJson();
