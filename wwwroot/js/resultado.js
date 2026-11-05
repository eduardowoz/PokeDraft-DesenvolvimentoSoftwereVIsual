const parametros = new URLSearchParams(window.location.search);
const partidaId = parametros.get("id");
const p1 = parametros.get("p1");
const p2 = parametros.get("p2");

async function finalizarEArmazenarReset() {
    try {
        const resposta = await fetch(`/api/partida/resetar`, {
            method: "POST"
        });

        if (!resposta.ok) {
            console.error("Erro ao resetar o banco de dados.");
        }
    } catch (erro) {
        console.error("Erro de conexão ao tentar resetar:", erro);
    }
}

async function carregarResultado() {
    if (!partidaId || !p1 || !p2) {
        console.error("Parâmetros da partida ou pokémons ausentes.");
        return;
    }

    try {
        const resposta = await fetch(`/api/resultado/${partidaId}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ p1: p1, p2: p2 })
        });
        if (!resposta.ok) {
            const detalhe = await resposta.text();
            throw new Error(`Erro ao calcular o resultado da batalha. ${detalhe}`);
        }

        const dados = await resposta.json();
        renderizarResultado(dados);
    } catch (erro) {
        console.error(erro);
    }
}

function renderizarResultado(dados) {
    const jogadorUm = dados.jogadorUm;
    const jogadorDois = dados.jogadorDois;
    const rodadas = dados.rodadas;

    document.getElementById("nomePlacarUm").textContent  = jogadorUm.nome;
    document.getElementById("nomePlacarDois").textContent = jogadorDois.nome;

    let vitoriasUm = 0;
    let vitoriasDois = 0;

    const listaRodadas = document.getElementById("listaRodadas");
    listaRodadas.innerHTML = "";

    rodadas.forEach(r => {
        if (r.vencedorId === jogadorUm.id){
            vitoriasUm++;
        } else if (r.vencedorId === jogadorDois.id){
            vitoriasDois++;
        }

        const card = document.createElement("div");
        card.className = "round-card";

        const isVencedorUm = r.vencedorId === jogadorUm.id;
        const isVencedorDois = r.vencedorId === jogadorDois.id;

        const pUm = r.pokemonUm;
        const pDois = r.pokemonDois;

        card.innerHTML = `
            <div class="lado-rodada ${isVencedorUm ? 'vencedor' : ''}">
                <div class="rodada-sprite" style="background:${fundoTipos(pUm.primeiroTipo, pUm.segundoTipo)}">
                    <img src="${pUm.spriteUrl}" alt="${pUm.nome}" />
                </div>
                <div class="rodada-info">
                    <div class="rodada-nome">${pUm.nome}</div>
                    <div class="rodada-tipos">${badgesTipo(pUm.primeiroTipo, pUm.segundoTipo)}</div>
                    <div class="rodada-overall">Overall <strong>${r.overallUm}</strong></div>
                </div>
                <div class="rodada-barra" style="background:${gradienteTipos(pUm.primeiroTipo, pUm.segundoTipo)}"></div>
            </div>

            <div class="rodada-centro">
                <span class="rodada-numero">Rodada ${r.rodada}</span>
                <span class="rodada-placar">${r.overallUm} <i>x</i> ${r.overallDois}</span>
                <span class="badge ${isVencedorUm ? 'bg-primary' : (isVencedorDois ? 'bg-danger' : 'bg-secondary')} rodada-vencedor">
                    ${r.vencedor === 'Empate' ? 'Empate' : r.vencedor}
                </span>
            </div>

            <div class="lado-rodada direita ${isVencedorDois ? 'vencedor' : ''}">
                <div class="rodada-info text-end">
                    <div class="rodada-nome">${pDois.nome}</div>
                    <div class="rodada-tipos">${badgesTipo(pDois.primeiroTipo, pDois.segundoTipo)}</div>
                    <div class="rodada-overall">Overall <strong>${r.overallDois}</strong></div>
                </div>
                <div class="rodada-sprite" style="background:${fundoTipos(pDois.primeiroTipo, pDois.segundoTipo)}">
                    <img src="${pDois.spriteUrl}" alt="${pDois.nome}" />
                </div>
                <div class="rodada-barra" style="background:${gradienteTipos(pDois.primeiroTipo, pDois.segundoTipo)}"></div>
            </div>
        `;

        listaRodadas.appendChild(card);
    });

    document.getElementById("pontosUm").textContent = vitoriasUm;
    document.getElementById("pontosDois").textContent = vitoriasDois;

    const vencedorContainer = document.getElementById("vencedorFinalContainer");
    const vencedorTexto = document.getElementById("vencedorFinalTexto");
    vencedorContainer.style.display = "block";

    if (vitoriasUm > vitoriasDois) {
        vencedorTexto.textContent = `Vencedor da Batalha: ${jogadorUm.nome}!`;
    } else if (vitoriasDois > vitoriasUm) {
        vencedorTexto.textContent = `Vencedor da Batalha: ${jogadorDois.nome}!`;
    } else {
        // Desempate por overall do time: soma o overall de TODAS as rodadas
        // (nao so das vencidas) e compara. Como os dois times tem o mesmo numero
        // de rodadas, comparar a soma equivale a comparar a media.
        let somaJogadorUm = 0;
        let somaJogadorDois = 0;
        rodadas.forEach(r => {
            somaJogadorUm += r.overallUm;
            somaJogadorDois += r.overallDois;
        });

        const mediaUm = (somaJogadorUm / rodadas.length).toFixed(1);
        const mediaDois = (somaJogadorDois / rodadas.length).toFixed(1);

        if (somaJogadorUm > somaJogadorDois) {
            vencedorTexto.textContent =
                `Empate no placar - vence ${jogadorUm.nome} pela media de overall (${mediaUm} x ${mediaDois})!`;
        } else if (somaJogadorDois > somaJogadorUm) {
            vencedorTexto.textContent =
                `Empate no placar - vence ${jogadorDois.nome} pela media de overall (${mediaDois} x ${mediaUm})!`;
        } else {
            vencedorTexto.textContent = `Resultado da Batalha: Empate!`;
        }
    }
}

document.getElementById("botaoVoltarInicio").addEventListener("click", async () => {
    await finalizarEArmazenarReset();
    window.location.href = "/";
});

carregarResultado();