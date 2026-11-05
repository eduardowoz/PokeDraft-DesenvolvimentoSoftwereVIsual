const parametros = new URLSearchParams(window.location.search);
const partidaId = parametros.get("id");

let jogadorUm;
let jogadorDois;

function atualizarBotaoBatalha() {
    const seletoresUm = [...document.querySelectorAll("#seletoresJogadorUm select")];
    const seletoresDois = [...document.querySelectorAll("#seletoresJogadorDois select")];

    const todosEscolhidos =
        seletoresUm.length === 6 &&
        seletoresDois.length === 6 &&
        seletoresUm.every(select => select.value !== "") &&
        seletoresDois.every(select => select.value !== "");

    const botao = document.getElementById("botaoIniciarResultado");
    const mensagem = document.getElementById("mensagemBatalha");

    botao.disabled = !todosEscolhidos;
    if (todosEscolhidos) {
        mensagem.textContent = "Tudo pronto para a batalha!";
        mensagem.classList.remove("text-danger");
        mensagem.classList.add("text-success");
    } else {
        mensagem.textContent = "Cada jogador precisa escolher 6 Pokémon.";
        mensagem.classList.remove("text-success");
        mensagem.classList.add("text-danger");
    }
}

function atualizarOpcoes(containerId) {
    const seletores = [...document.querySelectorAll(`#${containerId} select`)];
    const escolhidos = seletores.map(select => select.value).filter(valor => valor !== "");

    seletores.forEach(select => {
        [...select.options].forEach(opcao => {
            if (opcao.value === "") return;
            opcao.disabled = escolhidos.includes(opcao.value) && opcao.value !== select.value;
        });
    });

    atualizarBotaoBatalha();
}

const SPRITE_VAZIO =
    "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";

function criarSeletores(conexoes, containerId) {
    const container = document.getElementById(containerId);
    container.innerHTML = "";

    for (let i = 1; i <= 6; i++) {
        const slot = document.createElement("div");
        slot.className = "slot-batalha";

        const barra = document.createElement("div");
        barra.className = "slot-barra-tipo";

        const caixaSprite = document.createElement("div");
        caixaSprite.className = "slot-sprite vazio";
        caixaSprite.textContent = "?";

        const imagem = document.createElement("img");
        imagem.alt = `Pokémon do slot ${i}`;
        imagem.src = SPRITE_VAZIO;
        imagem.hidden = true;

        const info = document.createElement("div");
        info.className = "slot-info";

        const cabecalho = document.createElement("div");
        cabecalho.className = "slot-cabecalho";
        const numero = document.createElement("span");
        numero.className = "slot-numero";
        numero.textContent = `${i}º`;
        const nome = document.createElement("span");
        nome.className = "slot-nome vazio";
        nome.textContent = "Vazio";
        cabecalho.appendChild(numero);
        cabecalho.appendChild(nome);

        const tipos = document.createElement("div");
        tipos.className = "slot-tipos";

        const select = document.createElement("select");
        select.setAttribute("aria-label", `Pokémon ${i}`);
        select.innerHTML = `<option value="">Selecione o ${i}º Pokémon</option>`;

        conexoes.forEach(conexao => {
            const opcao = document.createElement("option");
            opcao.value = conexao.id;
            opcao.textContent = conexao.nome;
            opcao.dataset.spriteUrl = conexao.spriteUrl || "";
            opcao.dataset.primeiroTipo = conexao.primeiroTipo || "";
            opcao.dataset.segundoTipo = conexao.segundoTipo || "";
            select.appendChild(opcao);
        });

        select.addEventListener("change", () => {
            const opcao = select.options[select.selectedIndex];
            const escolheu = Boolean(select.value);

            if (!escolheu) {
                imagem.hidden = true;
                imagem.src = SPRITE_VAZIO;
                caixaSprite.classList.add("vazio");
                caixaSprite.textContent = "?";
                caixaSprite.appendChild(imagem);
                caixaSprite.style.background = "";
                nome.textContent = "Vazio";
                nome.classList.add("vazio");
                tipos.innerHTML = "";
                barra.style.background = "";
            } else {
                const primeiro = opcao.dataset.primeiroTipo;
                const segundo = opcao.dataset.segundoTipo;

                caixaSprite.textContent = "";
                caixaSprite.classList.remove("vazio");
                caixaSprite.appendChild(imagem);
                imagem.hidden = false;
                imagem.src = opcao.dataset.spriteUrl || SPRITE_VAZIO;
                imagem.alt = opcao.textContent;
                caixaSprite.style.background = fundoTipos(primeiro, segundo);

                nome.textContent = opcao.textContent;
                nome.classList.remove("vazio");
                tipos.innerHTML = badgesTipo(primeiro, segundo);
                barra.style.background = gradienteTipos(primeiro, segundo);
            }

            atualizarOpcoes(containerId);
        });

        caixaSprite.appendChild(imagem);
        info.appendChild(cabecalho);
        info.appendChild(tipos);
        info.appendChild(select);
        slot.appendChild(caixaSprite);
        slot.appendChild(info);
        slot.appendChild(barra);
        container.appendChild(slot);
    }
}

async function carregarConexoes(jogador) {
    const resposta = await fetch(`/api/conexao/${partidaId}/${jogador.id}`);
    if (!resposta.ok) {
        throw new Error("Erro ao carregar os Pokémon do jogador.");
    }
    return await resposta.json();
}

async function carregarBatalha() {
    if (!partidaId) {
        console.error("ID da partida não encontrado na URL.");
        return;
    }

    try {
        const respostaPartida = await fetch(`/api/partida/${partidaId}`);
        if (!respostaPartida.ok) throw new Error("Erro ao carregar a partida.");
        const partida = await respostaPartida.json();

        const respostaJogadores = await fetch("/api/jogador");
        if (!respostaJogadores.ok) throw new Error("Erro ao carregar os jogadores.");
        const jogadores = await respostaJogadores.json();

        jogadorUm = jogadores.find(j => j.id === partida.jogadorUmId);
        jogadorDois = jogadores.find(j => j.id === partida.jogadorDoisId);

        if (!jogadorUm || !jogadorDois) {
            throw new Error("Não foi possível encontrar os dois jogadores.");
        }

        document.getElementById("nomeJogadorUm").textContent = jogadorUm.nome;
        document.getElementById("spriteJogadorUm").src = jogadorUm.spriteUrl || "";

        document.getElementById("nomeJogadorDois").textContent = jogadorDois.nome;
        document.getElementById("spriteJogadorDois").src = jogadorDois.spriteUrl || "";

        const [conexoesUm, conexoesDois] = await Promise.all([
            carregarConexoes(jogadorUm),
            carregarConexoes(jogadorDois)
        ]);

        criarSeletores(conexoesUm, "seletoresJogadorUm");
        criarSeletores(conexoesDois, "seletoresJogadorDois");

        atualizarBotaoBatalha();

    } catch (erro) {
        console.error(erro);
    }
}

document.getElementById("botaoIniciarResultado").addEventListener("click", () => {
    const seletoresUm = [...document.querySelectorAll("#seletoresJogadorUm select")];
    const seletoresDois = [...document.querySelectorAll("#seletoresJogadorDois select")];
    const p1 = seletoresUm.map(s => s.value).join(",");
    const p2 = seletoresDois.map(s => s.value).join(",");
    window.location.href = `/Home/Resultado?id=${partidaId}&p1=${p1}&p2=${p2}`;
});

carregarBatalha();