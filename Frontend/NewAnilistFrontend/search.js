console.log("Loaded script:", document.currentScript.src);





async function doSearch(){

try{
const searchInput = document.getElementById("site-search").value;


const response = await fetch("http://localhost:5243/Show/search-animes", {
method: "POST",
headers: {"Content-Type": "application/json"},
body: JSON.stringify({ searchTerm: searchInput})

});

if(!response.ok) throw new Error("Network response was not ok");


const json = await response.json();

console.log("Response data:", json);
renderResults(json);
} catch (error){

    console.error('error', error);

}


}

function renderResults(json){

const resultContainer = document.getElementById("results");

resultContainer.innerHTML = "";

json.forEach(anime => {

const resultDiv = document.createElement("div")

resultDiv.innerHTML = `<a href="../details.html?id=${anime.id}">
                <h3>${anime.title.english ?? anime.title.romaji} </h3>  </a>`;

resultContainer.appendChild(resultDiv);
});
}



document.getElementById("searchButton").addEventListener("click",()=>{


doSearch(false);


});
