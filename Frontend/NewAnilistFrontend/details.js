

async function fetchShow() {
  try {
    const container = document.getElementById('outputs');
    if (!container) return;

    const urlParams = new URLSearchParams(window.location.search);
    const id = parseInt(urlParams.get('id'), 10);
    if (!id) return;

    const response = await fetch(`http://localhost:5243/Show/get-show?id=${id}`, {
      method: "GET",
  
    });

    if (!response.ok) throw new Error("Network response was not ok");

    const data = await response.json();

    container.innerHTML = `
      <h3>${data.title.english ?? data.title.romaji}</h3>
     
      <img src="${data.coverImage.large}" alt="${data.title.romaji}">
         <h3>${data.genres}</h3>
       <h3>${data.description}</h3>
    `;  


  } catch (error) {
    console.error('Error:', error);
  }
}

fetchShow();
