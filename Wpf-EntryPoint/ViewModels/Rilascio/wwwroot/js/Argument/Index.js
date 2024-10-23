async function insertHtmlStructure() {
    const container = document.getElementById('container-234-parent');
    if (!container) {
        console.error('Container not found.');
        return;
    }

    container.style.height = '';
    const personDataElement = document.getElementById('id-argomento');
    if (!personDataElement || !personDataElement.hasAttribute('data-model')) {
        console.error('Data model element or attribute not found.');
        return;
    }

    const idArgomento = personDataElement.getAttribute('data-model');
    let myData = await fetchAndProcessData(idArgomento);
    if (myData.Contenuti < 1) {
        return;
    }

    const g = popolaHTML(myData.Contenuti, idArgomento);
    container.appendChild(g);

    resizeView();
}

async function fetchAndProcessData(id_argomento) {
    try {
        const data = await grab9More(id_argomento);
        console.log('Dati ottenuti:', data);
        return data;
    } catch (error) {
        console.error('Errore durante la richiesta:', error);
        return [];
    }
}

function popolaHTML(myData, idArgomento) {
    const containerClass234 = document.createElement('div');
    containerClass234.className = 'row container-class234 children-div-all second-lv-conteiner';

    for (let i = 0; i < myData.length; i++) {
        const divType = document.createElement('div');
        divType.className = 'item-argument';
        divType.setAttribute('data-index', i);

        const form = document.createElement('form');
        form.action = '/Argument/Article';
        form.method = 'post';
        form.className = 'w-100 h-100';

        const input1 = document.createElement('input');
        input1.type = 'hidden';
        input1.name = 'id_articolo';
        input1.value = myData[i].Title;

        const input2 = document.createElement('input');
        input2.type = 'hidden';
        input2.name = 'id_argomento';
        input2.value = idArgomento;

        const button = document.createElement('button');
        button.type = 'submit';
        button.className = 'invisible-button';

        const img = document.createElement('img');
        img.src = myData[i].ImgPath;
        img.alt = 'Immagine';

        const span = document.createElement('span');
        span.classList.add('colore-scritte-LV');
        span.textContent = myData[i].Title;

        button.appendChild(img);
        button.appendChild(span);
        form.appendChild(input1);
        form.appendChild(input2);
        form.appendChild(button);
        divType.appendChild(form);
        containerClass234.appendChild(divType);
    }

    return containerClass234;
}
