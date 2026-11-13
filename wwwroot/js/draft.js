const parametros = new URLSearchParams(window.location.search);
const partidaId = parametros.get("id");
let jogadorUm;
let jogadorDois;
let jogadorTurno;

const TAMANHO_TIME = 6;
let quantidadeTime = {};   // { [jogadorId]: quantos pokemons ja tem }

async function carregarPokemonsJogador(jogador, elementoId, selectId, valorId){
    const resposta = await fetch(
        `/api/conexao/${partidaId}/${jogador.id}`
    );
    if(!resposta.ok){
        mostrarAviso("Erro ao carregar os Pokémon do jogador.");
        return 0;
    }
    const pokemons = await resposta.json();
    const lista = document.getElementById(elementoId);
    lista.innerHTML = "";
    pokemons.forEach(pokemon => {
        if (!pokemon.spriteUrl){
            return;
        }
        const imagem = document.createElement("img");
        imagem.src = pokemon.spriteUrl;
        imagem.alt = pokemon.nome;
        imagem.title = pokemon.segundoTipo
            ? `${pokemon.nome} (${nomeDoTipo(pokemon.primeiroTipo)}/${nomeDoTipo(pokemon.segundoTipo)})`
            : `${pokemon.nome} (${nomeDoTipo(pokemon.primeiroTipo)})`;
        imagem.loading = "lazy";
        imagem.style.borderBottomColor = corDoTipo(pokemon.primeiroTipo);
        imagem.style.background = fundoTipos(pokemon.primeiroTipo, pokemon.segundoTipo);
        lista.appendChild(imagem);
    });

    const select = document.getElementById(selectId);
    select.innerHTML = '<option value="">Selecione um Pokémon</option>';

    pokemons.forEach(pokemon => {
        const opcao = document.createElement("option");
        opcao.value = pokemon.conexaoId;
        opcao.textContent = pokemon.nome;
        opcao.dataset.custo = pokemon.custo;
        select.appendChild(opcao);
    });

    quantidadeTime[jogador.id] = pokemons.length;

    const valor = document.getElementById(valorId);
    select.onchange = () => {
        const opcao = select.options[select.selectedIndex];
        if(!opcao.value){
            valor.textContent = "0";
            return;
        }
        const custo = Number(opcao.dataset.custo);
        valor.textContent = Math.floor(custo * 0.75);
    };
    return pokemons.length;
}

async function carregarPokemonsDosJogadores(){
    const [quantidadeJogadorUm, quantidadeJogadorDois] =
        await Promise.all([
            carregarPokemonsJogador(
                jogadorUm,"pokemonsJogadorUm","selectVendaJogadorUm","valorVendaJogadorUm"
            ),
            carregarPokemonsJogador(
                jogadorDois,"pokemonsJogadorDois","selectVendaJogadorDois","valorVendaJogadorDois"
            )
        ]);
    const botao = document.getElementById("botaoIniciarDraftItens");
    const mensagem = document.getElementById("mensagemDraftItens");
    const podeIniciar =
        quantidadeJogadorUm === TAMANHO_TIME && quantidadeJogadorDois === TAMANHO_TIME;

    botao.disabled = !podeIniciar;
    mensagem.hidden = podeIniciar;
    if(!podeIniciar){
        mensagem.textContent =
            `Cada jogador precisa de ${TAMANHO_TIME} Pokémon ` +
            `(${jogadorUm.nome}: ${quantidadeJogadorUm}/${TAMANHO_TIME} · ` +
            `${jogadorDois.nome}: ${quantidadeJogadorDois}/${TAMANHO_TIME}).`;
    }
}

async function venderPokemon(selectId){
    const select = document.getElementById(selectId);
    if(!select.value){
        return mostrarAviso("Selecione um Pokémon para vender.");
    }
    const conexaoId = select.value;
    const resposta = await fetch(`/api/conexao/${conexaoId}`, {
        method: "DELETE"
    });
    if(!resposta.ok){
        mostrarAviso(await resposta.text() || "Não foi possível vender o Pokémon.");
        return;
    }
    await carregarJogadores();
}

function atualizarTurno(){
    const indicador = document.getElementById("indicadorVez");

    if(jogadorTurno === jogadorUm.id){
        indicador.textContent = `Turno do Jogador: ${jogadorUm.nome}`;
    } else {
        indicador.textContent = `Turno do Jogador: ${jogadorDois.nome}`;
    }
}

function mostrarPokemons(pokemons){
    const lista = document.getElementById("listaPokemons");
    lista.innerHTML = "";

    pokemons.forEach(pokemon => {
        const card = document.createElement("div");
        card.classList.add("card-pokemon");

        card.addEventListener("click", async () => {
            const lista = await fetch(
                `/api/conexao/${partidaId}/${jogadorTurno}`
            );
            const listapokemons = await lista.json();
            quantidadeTime[jogadorTurno] = listapokemons.length;
            if((quantidadeTime[jogadorTurno] ?? 0) >= TAMANHO_TIME){
                mostrarAviso(
                    `Time cheio: cada jogador pode ter no máximo ${TAMANHO_TIME} Pokémon.`
                );
                return;
            }

            const resposta = await fetch("/api/conexao", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    PartidaId: Number(partidaId),
                    JogadorId: jogadorTurno,
                    PokemonId: pokemon.id
                })
            });
            if (!resposta.ok) {
                const erro = await resposta.text();
                mostrarAviso(erro || "Não foi possível adicionar esse Pokémon.");
                return;
            }
            await carregarJogadores();
        });

        const caixaSprite = document.createElement("div");
        caixaSprite.className = "caixa-sprite";
        caixaSprite.style.background = fundoTipos(
            pokemon.primeiroTipo, pokemon.segundoTipo
        );

        const imagem = document.createElement("img");
        imagem.src = pokemon.spriteUrl;
        imagem.alt = pokemon.nome;
        imagem.loading = "lazy";
        caixaSprite.appendChild(imagem);

        const nome = document.createElement("h3");
        nome.textContent = pokemon.nome;

        const tipos = document.createElement("div");
        tipos.className = "tipos-card";
        tipos.innerHTML = badgesTipo(pokemon.primeiroTipo, pokemon.segundoTipo);

        const custo = document.createElement("p");
        custo.className = "custo-card";
        custo.innerHTML = `Custo: <span>${pokemon.custo}</span>`;

        const barra = document.createElement("div");
        barra.className = "barra-tipo";
        barra.style.background = gradienteTipos(
            pokemon.primeiroTipo, pokemon.segundoTipo
        );

        card.appendChild(caixaSprite);
        card.appendChild(nome);
        card.appendChild(tipos);
        card.appendChild(custo);
        card.appendChild(barra);

        lista.appendChild(card);
    });
}

async function sortearNovamente(opcao){
    const resposta = await fetch(`/api/pokemon/${opcao}/${partidaId}`, {
            method: "POST"
        }
    );
    if(!resposta.ok){
        console.error("Erro ao sortear novamente:", resposta.status);
        return;
    }
    const pokemons = await resposta.json();
    mostrarPokemons(pokemons);
}

async function carregarDraft(){
    const resposta = await fetch(`/api/pokemon/${partidaId}`);
    if(!resposta.ok){
        console.error("Erro ao buscar os Pokémon:", resposta.status);
        return;
    }
    const pokemons = await resposta.json();
    mostrarPokemons(pokemons);
}

async function carregarJogadores(){
    const respostaPartida = await fetch(`/api/partida/${partidaId}`);
    if(!respostaPartida.ok){
        console.error("Erro ao buscar a partida:", respostaPartida.status);
        return;
    }
    const partida = await respostaPartida.json();
    console.log("Dados da partida:", partida);

    const respostaJogadores = await fetch("/api/jogador");
    if(!respostaJogadores.ok){
        console.error("Erro ao buscar jogadores:", respostaJogadores.status);
        return;
    }
    const jogadores = await respostaJogadores.json();
    jogadorUm = jogadores.find(
        jogador => jogador.id === partida.jogadorUmId
    );
    // find é tipo um forEach que dá return assim que a comparação der certo
    // economiza o: break; como também: uma váriavel extra com o valor do JogadorId
    jogadorDois = jogadores.find(
        jogador => jogador.id === partida.jogadorDoisId
    );
    if(!jogadorUm || !jogadorDois){
        console.error("Não foi possível encontrar os dois jogadores.");
        return;
    }
    console.log("Jogador 1:", jogadorUm, "Jogador 2:", jogadorDois);

    // se quem está na vez já tem o time cheio, passa a vez para o outro jogador
    const poke = await fetch(
        `/api/conexao/${partida.id}/${partida.turno}`
    );
    const pokes = await poke.json();
    const attTurno = await fetch(
        `/api/conexao/${partida.id}/${pokes.length}`,
        {
            method: "PATCH"
        }
    );
    console.log(attTurno);

    // atualizar o id do jogador
    jogadorTurno = partida.turno;
    // o PATCH pode ter trocado o turno no banco; sem isso a tela continuaria
    // mostrando (e usando) o jogador antigo, que já está com o time cheio
    if(attTurno.ok && pokes.length >= TAMANHO_TIME){
        jogadorTurno = partida.turno === jogadorUm.id ? jogadorDois.id : jogadorUm.id;
    }

    atualizarTurno();
    // mostrar os jogadores
    document.getElementById("nomeJogadorUm").textContent = jogadorUm.nome;
    document.getElementById("spriteJogadorUm").src = jogadorUm.spriteUrl;
    document.getElementById("budgetJogadorUm").textContent = jogadorUm.budget;
    document.getElementById("nomeJogadorDois").textContent = jogadorDois.nome;
    document.getElementById("spriteJogadorDois").src = jogadorDois.spriteUrl;
    document.getElementById("budgetJogadorDois").textContent = jogadorDois.budget;

    await carregarPokemonsDosJogadores();
    await carregarDraft();
}

async function carregarPartida(){
    if(!partidaId){
        console.error("ID da partida não encontrado na URL.");
        return;
    }
    const startBudget = await fetch(
        `/api/partida/${partidaId}`,
        {
            method: "PATCH"
        }
    );
    if(!startBudget.ok){
        console.error("Erro ao inicializar os orçamentos.");
        return;
    }
    console.log("Orçamentos estão inicializados.");

    await carregarJogadores();
}

document.getElementById("botaoVendaJogadorUm")
    .addEventListener("click", async () => {
        await venderPokemon("selectVendaJogadorUm", "valorVendaJogadorUm");
    });
document.getElementById("botaoVendaJogadorDois")
    .addEventListener("click", async () => {
        await venderPokemon("selectVendaJogadorDois", "valorVendaJogadorDois");
    });
document.getElementById("botaoNovoTipo")
    .addEventListener("click", () => {
        sortearNovamente("tipo");
    });
document.getElementById("botaoNovaGeracao")
    .addEventListener("click", () => {
        sortearNovamente("geracao");
    });
document.getElementById("botaoIniciarDraftItens")
    .addEventListener("click", () => {
        window.location.href = `/Home/Itens?id=${partidaId}`;
    });

carregarPartida();