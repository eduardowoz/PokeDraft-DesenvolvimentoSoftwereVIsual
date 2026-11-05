async function criarJogador(){
    const campoNome = document.getElementById("nomeJogador");
    const nome = campoNome.value.trim();
    if(nome === ""){
        mostrarAviso("Digite o nome do jogador.");
        return;
    }

    const campoSprite = document.getElementById("spriteJogador");
    const spriteUrl = campoSprite.value;
    if(spriteUrl === ""){
        mostrarAviso("Selecione uma sprite para o jogador.");
        return;
    }

    const resposta = await fetch("/api/jogador", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Nome: nome,
            SpriteUrl: spriteUrl
        })
    });
    if(!resposta.ok){
        const erro = await resposta.text();
        mostrarAviso(erro || "Não foi possível cadastrar o jogador.");
        return;
    }

    campoNome.value = "";
    campoSprite.value = "";
    document.getElementById("previewContainer").innerHTML = '<span class="text-muted text-center" style="font-size: 0.7rem;">Preview</span>';
    
    await carregarJogadores();
}

async function carregarJogadores(){
    const resposta = await fetch("/api/jogador");
    if(!resposta.ok){
        console.error("Erro ao buscar jogadores:", resposta.status);
        return;
    }

    const jogadores = await resposta.json();
    const lista = document.getElementById("listaJogadores");
    const contadorBadge = document.getElementById("contadorJogadores");

    contadorBadge.textContent = `${jogadores.length} cadastrado${jogadores.length === 1 ? '' : 's'}`;
    
    lista.innerHTML = "";
    if (jogadores.length === 0) {
        lista.innerHTML = `<p class="text-muted small text-center py-3 m-0">Nenhum jogador cadastrado ainda.</p>`;
    }
    jogadores.forEach(jogador => {
        const item = document.createElement("div");
        item.className = "jogador-item d-flex align-items-center justify-content-between";
        
        let imagemHtml = "";
        if(jogador.spriteUrl){
            imagemHtml = `<img src="${jogador.spriteUrl}" alt="Sprite de ${jogador.nome}" class="jogador-avatar shadow-sm" loading="lazy" />`;
        } else {
            imagemHtml = `<div class="jogador-avatar d-flex align-items-center justify-content-center text-muted fw-bold" style="font-size: 0.8rem;">?</div>`;
        }

        item.innerHTML = `
            <div class="d-flex align-items-center gap-3">
                ${imagemHtml}
                <div>
                    <h6 class="mb-0 fw-bold text-dark" style="font-size: 0.95rem;">${jogador.nome}</h6>
                    <small class="text-muted" style="font-size: 0.8rem;">ID: #${jogador.id}</small>
                </div>
            </div>
            <button type="button" class="btn-deletar-jogador" onclick="excluirJogador(${jogador.id})" title="Excluir jogador">
                <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>
            </button>
        `;

        lista.appendChild(item);
    });

    const jogadorUm = document.getElementById("jogadorUm");
    const jogadorDois = document.getElementById("jogadorDois");

    jogadorUm.innerHTML = '<option value="">Escolha o primeiro jogador</option>';
    jogadorDois.innerHTML = '<option value="">Escolha o segundo jogador</option>';

    jogadores.forEach(jogador => {
        const opcaoUm = document.createElement("option");
        opcaoUm.value = jogador.id;
        opcaoUm.textContent = jogador.nome;
        jogadorUm.appendChild(opcaoUm);

        const opcaoDois = document.createElement("option");
        opcaoDois.value = jogador.id;
        opcaoDois.textContent = jogador.nome;
        jogadorDois.appendChild(opcaoDois);
    });
}

async function excluirJogador(id) {
    const resposta = await fetch(`/api/jogador/${id}`, {
        method: "DELETE"
    });
    if (!resposta.ok) {
        const erro = await resposta.text();
        mostrarAviso(erro || "Não foi possível excluir o jogador.");
        return;
    }

    await carregarJogadores();
}

async function carregarSprites() {
    const resposta = await fetch("/api/jogador/sprites");
    if(!resposta.ok){
        console.error("Erro ao carregar as sprites.");
        return;
    }

    const sprites = await resposta.json();
    const campoSprite = document.getElementById("spriteJogador");
    campoSprite.innerHTML = '<option value="">Selecione uma sprite</option>';

    sprites.forEach(spriteUrl => {
        const opcao = document.createElement("option");
        opcao.value = spriteUrl;
        opcao.textContent = spriteUrl.split("/").pop().replace(/\.[^/.]+$/, "");
        campoSprite.appendChild(opcao);
    });
}

function atualizarPreviewSprite() {
    const campoSprite = document.getElementById("spriteJogador");
    const previewContainer = document.getElementById("previewContainer");
    const spriteUrl = campoSprite.value;
    if (!spriteUrl) {
        previewContainer.innerHTML = '<span class="text-muted text-center px-1" style="font-size: 0.7rem;">Nenhum</span>';
        return;
    }
    previewContainer.innerHTML = `<img src="${spriteUrl}" alt="Preview" width="56" height="56" style="object-fit: contain; image-rendering: pixelated;" />`;
}

const BUDGET_MINIMO = 1000;

async function criarPartida(){
    const jogadorUmId = Number(document.getElementById("jogadorUm").value);
    const jogadorDoisId = Number(document.getElementById("jogadorDois").value);
    const campoBudget = document.getElementById("budget");
    const budget = Number(campoBudget.value);

    if(!jogadorUmId || !jogadorDoisId){
        mostrarAviso("Escolha os dois jogadores da partida.");
        return;
    }
    if(jogadorUmId === jogadorDoisId){
        mostrarAviso("Escolha dois jogadores diferentes para a partida.");
        return;
    }
    if(!campoBudget.value.trim() || Number.isNaN(budget)){
        mostrarAviso("Informe o budget da partida.");
        campoBudget.focus();
        return;
    }
    if(budget < BUDGET_MINIMO){
        mostrarAviso(`Budget inferior ao mínimo: informe ao menos ${BUDGET_MINIMO} pts.`);
        campoBudget.focus();
        campoBudget.select();
        return;
    }

    const resposta = await fetch("/api/partida", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            jogadorUmId: jogadorUmId,
            jogadorDoisId: jogadorDoisId,
            budget: budget
        })
    });
    if(!resposta.ok){
        const erroTexto = await resposta.text();
        mostrarAviso(erroTexto || "Não foi possível criar a partida.");
        return;
    }

    const partida = await resposta.json();
    window.location.href = `/Home/Draft?id=${partida.id}`;
}

document.addEventListener("DOMContentLoaded", () => {
    carregarSprites();
    carregarJogadores();
});