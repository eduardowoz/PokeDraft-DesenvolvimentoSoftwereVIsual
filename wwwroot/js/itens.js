const parametros = new URLSearchParams(window.location.search);
const partidaId = parametros.get("id");
let jogadorUm;
let jogadorDois;
let jogadorTurno;

async function carregarConexoesJogador(jogador, equipeId, selectId, itensId){
    const resposta = await fetch(
        `/api/conexao/${partidaId}/${jogador.id}`
    );
    if(!resposta.ok){
        mostrarAviso("Erro ao carregar os Pokémon do jogador.");
        return null;
    }

    const conexoes = await resposta.json();
    const equipe = document.getElementById(equipeId);
    equipe.innerHTML = "";
    conexoes.forEach(conexao => {
        const imagem = document.createElement("img");
        imagem.src = conexao.spriteUrl;
        imagem.alt = conexao.nome;
        imagem.title = conexao.nome;
        imagem.loading = "lazy";

        equipe.appendChild(imagem);
    });

    // 1 item por Pokémon: quem ja tem item nao entra na lista de destino
    const semItem = conexoes.filter(conexao => conexao.itemId == null);
    const select = document.getElementById(selectId);
    select.innerHTML = semItem.length === 0
        ? '<option value="">Todos os Pokémon já têm item</option>'
        : '<option value="">Selecione um Pokémon</option>';
    select.disabled = semItem.length === 0;
    semItem.forEach(conexao => {
        const opcao = document.createElement("option");
        opcao.value = conexao.conexaoId;
        opcao.textContent = conexao.nome;

        select.appendChild(opcao);
    });
    
    const listaItens = document.getElementById(itensId);
    listaItens.innerHTML = "";
    conexoes.filter(conexao => conexao.itemId != null)
        .forEach(conexao => {
            if(conexao.itemSpriteUrl){
                const imagem = document.createElement("img");
                imagem.src = conexao.itemSpriteUrl;
                imagem.alt = conexao.itemNome;
                imagem.title = conexao.itemNome;
                imagem.loading = "lazy";

                listaItens.appendChild(imagem);
            } else {
                const nome = document.createElement("span");
                nome.textContent = conexao.itemNome;
                listaItens.appendChild(nome);
            }
        });
    return conexoes;
}

function atualizarTurno() {
    const indicador = document.getElementById("indicadorVez");
    if(jogadorTurno === jogadorUm.id){
        indicador.textContent = `Turno do Jogador: ${jogadorUm.nome}`;
    } else {
        indicador.textContent = `Turno do Jogador: ${jogadorDois.nome}`;
    }
}

async function carregarItensDisponiveis() {
    const resposta = await fetch(`/api/item/${partidaId}`);
    if(!resposta.ok){
        mostrarAviso("Erro ao carregar os itens disponíveis.");
        return;
    }
    const itens = await resposta.json();
    mostrarItens(itens);
}

function mostrarItens(itens) {
    const lista = document.getElementById("listaItens");
    lista.innerHTML = "";

    itens.forEach(item => {
        const card = document.createElement("div");
        card.classList.add("card-item");
        card.addEventListener("click", async () => {
            await comprarItem(item);
        });

        const caixaSprite = document.createElement("div");
        caixaSprite.className = "caixa-sprite";
        caixaSprite.style.background = item.tipo
            ? fundoTipos(item.tipo, null)
            : "linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%)";

        const imagem = document.createElement("img");
        imagem.src = item.spriteUrl;
        imagem.alt = item.nome;
        imagem.loading = "lazy";
        caixaSprite.appendChild(imagem);

        const nome = document.createElement("h3");
        nome.textContent = item.nome;

        const tipos = document.createElement("div");
        tipos.className = "tipos-card";
        // item sem tipo serve para qualquer Pokémon
        tipos.innerHTML = item.tipo
            ? badgesTipo(item.tipo, null)
            : '<span class="badge-sem-tipo">Universal</span>';

        const stats = document.createElement("div");
        stats.className = "linha-stats";
        stats.innerHTML =
            `<div>Custo <span>${item.custo}</span></div>` +
            `<div>Tier <span>${item.overall}</span></div>`;

        const barra = document.createElement("div");
        barra.className = "barra-tipo";
        barra.style.background = item.tipo
            ? gradienteTipos(item.tipo, null)
            : "linear-gradient(90deg, #94a3b8 0%, #cbd5e1 100%)";

        card.appendChild(caixaSprite);
        card.appendChild(nome);
        card.appendChild(tipos);
        card.appendChild(stats);
        card.appendChild(barra);

        lista.appendChild(card);
    });
}

async function comprarItem(item) {
    const jogador = jogadorTurno === jogadorUm.id
        ? jogadorUm
        : jogadorDois;

    const selectId = jogador.id === jogadorUm.id
        ? "selectPokemonItemJogadorUm"
        : "selectPokemonItemJogadorDois";

    const select = document.getElementById(selectId);
    if (!select.value) {
        mostrarAviso("Selecione um Pokémon para receber o item (só aparecem os que ainda estão sem item).");
        return;
    }

    const conexaoId = select.value;
    const resposta = await fetch(
        `/api/item/${partidaId}/${jogadorTurno}/${conexaoId}/${item.id}`,
        {
            method: "POST"
        }
    );
    if(!resposta.ok){
        const erro = await resposta.text();
        mostrarAviso(erro || "Não foi possível comprar o item.");
        return;
    }

    await carregarJogadores();
}

function atualizarBotaoIniciarBatalha(conexoesUm, conexoesDois){
    const quantidadeUm = conexoesUm.filter(
        conexao => conexao.itemId != null
    ).length;
    const quantidadeDois = conexoesDois.filter(
        conexao => conexao.itemId != null
    ).length;
    const podeIniciar = quantidadeUm >= 0 && quantidadeDois >= 0;
    document.getElementById("botaoIniciarBatalha").disabled = !podeIniciar;
    document.getElementById("mensagemIniciarBatalha").hidden = podeIniciar;
}

async function carregarJogadores(){
    const respostaPartida = await fetch(`/api/partida/${partidaId}`);
    if (!respostaPartida.ok) {
        console.error("Erro ao buscar a partida:", respostaPartida.status);
        return;
    }
    const partida = await respostaPartida.json();
    
    const respostaJogadores = await fetch("/api/jogador");
    if (!respostaJogadores.ok) {
        console.error("Erro ao buscar os jogadores:", respostaJogadores.status);
        return;
    }
    const jogadores = await respostaJogadores.json();
    jogadorUm = jogadores.find(
        jogador => jogador.id === partida.jogadorUmId
    );
    jogadorDois = jogadores.find(
        jogador => jogador.id === partida.jogadorDoisId
    );
    if(!jogadorUm || !jogadorDois){
        console.error("Não foi possível encontrar os jogadores.");
        return;
    }

    jogadorTurno = partida.turno;
    atualizarTurno();

    document.getElementById("nomeJogadorUm").textContent = jogadorUm.nome;
    document.getElementById("spriteJogadorUm").src = jogadorUm.spriteUrl;
    document.getElementById("budgetJogadorUm").textContent = jogadorUm.budget;
    document.getElementById("nomeJogadorDois").textContent = jogadorDois.nome;
    document.getElementById("spriteJogadorDois").src = jogadorDois.spriteUrl;
    document.getElementById("budgetJogadorDois").textContent = jogadorDois.budget;

    const [conexoesUm, conexoesDois] = await Promise.all([
        carregarConexoesJogador(
            jogadorUm,"equipeJogadorUm","selectPokemonItemJogadorUm","itensJogadorUm"
        ),
        carregarConexoesJogador(
            jogadorDois,"equipeJogadorDois","selectPokemonItemJogadorDois","itensJogadorDois"
        )
    ]);
    if(conexoesUm === null || conexoesDois === null){
        return;
    }
    atualizarBotaoIniciarBatalha(conexoesUm, conexoesDois);
    await carregarItensDisponiveis();
}

document.getElementById("botaoIniciarBatalha")
    .addEventListener("click", () => {
        window.location.href = `/Home/Batalha?id=${partidaId}`;
    });

carregarJogadores();