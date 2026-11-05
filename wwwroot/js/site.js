// Avisos visiveis para o usuario.
// Antes todo erro ia para console.error, entao a tela simplesmente nao respondia
// e o jogador nao tinha como saber o motivo. Este helper e carregado em todas as
// paginas (via _Layout) e fica disponivel para index.js, draft.js, itens.js, etc.

function obterContainerAvisos() {
    let container = document.getElementById("containerAvisos");
    if (!container) {
        container = document.createElement("div");
        container.id = "containerAvisos";
        container.className = "container-avisos";
        document.body.appendChild(container);
    }
    return container;
}

/**
 * Mostra um aviso flutuante no canto da tela.
 * @param {string} mensagem texto exibido
 * @param {"erro"|"sucesso"|"info"} tipo cor do aviso (padrao: "erro")
 */
function mostrarAviso(mensagem, tipo = "erro") {
    if (!mensagem) {
        return;
    }

    const container = obterContainerAvisos();

    // evita empilhar o mesmo aviso quando o usuario clica varias vezes
    const repetido = [...container.children].find(
        a => a.dataset.mensagem === mensagem
    );
    if (repetido) {
        repetido.classList.remove("aviso-entrada");
        void repetido.offsetWidth;          // reinicia a animacao
        repetido.classList.add("aviso-entrada");
        return;
    }

    const aviso = document.createElement("div");
    aviso.className = `aviso aviso-${tipo} aviso-entrada`;
    aviso.dataset.mensagem = mensagem;
    aviso.setAttribute("role", tipo === "erro" ? "alert" : "status");

    const texto = document.createElement("span");
    texto.textContent = mensagem;

    const fechar = document.createElement("button");
    fechar.type = "button";
    fechar.className = "aviso-fechar";
    fechar.setAttribute("aria-label", "Fechar aviso");
    fechar.textContent = "×";
    fechar.addEventListener("click", () => aviso.remove());

    aviso.appendChild(texto);
    aviso.appendChild(fechar);
    container.appendChild(aviso);

    setTimeout(() => {
        aviso.classList.add("aviso-saida");
        setTimeout(() => aviso.remove(), 300);
    }, 4000);
}

// ===== Cores dos tipos de Pokémon =====
// Paleta baseada nas cores classicas de cada tipo. Usada para colorir as barras
// dos Pokémon nas telas de draft, batalha e resultado.

const CORES_TIPO = {
    normal:   "#A8A77A",
    fire:     "#EE8130",
    water:    "#6390F0",
    electric: "#F7D02C",
    grass:    "#7AC74C",
    ice:      "#96D9D6",
    fighting: "#C22E28",
    poison:   "#A33EA1",
    ground:   "#E2BF65",
    flying:   "#A98FF3",
    psychic:  "#F95587",
    bug:      "#A6B91A",
    rock:     "#B6A136",
    ghost:    "#735797",
    dragon:   "#6F35FC",
    dark:     "#705746",
    steel:    "#B7B7CE",
    fairy:    "#D685AD"
};

const NOMES_TIPO = {
    normal: "Normal",   fire: "Fogo",       water: "Água",
    electric: "Elétrico", grass: "Planta",  ice: "Gelo",
    fighting: "Lutador", poison: "Venenoso", ground: "Terrestre",
    flying: "Voador",   psychic: "Psíquico", bug: "Inseto",
    rock: "Pedra",      ghost: "Fantasma",  dragon: "Dragão",
    dark: "Sombrio",    steel: "Aço",       fairy: "Fada"
};

const COR_TIPO_PADRAO = "#94a3b8";

function corDoTipo(tipo) {
    if (!tipo) {
        return COR_TIPO_PADRAO;
    }
    return CORES_TIPO[String(tipo).toLowerCase()] ?? COR_TIPO_PADRAO;
}

function nomeDoTipo(tipo) {
    if (!tipo) {
        return "";
    }
    const chave = String(tipo).toLowerCase();
    return NOMES_TIPO[chave] ?? chave;
}

/**
 * Faixa de cor do Pokémon. Com um tipo fica lisa; com dois tipos
 * a cor passa de um para o outro no meio da barra.
 */
function gradienteTipos(primeiroTipo, segundoTipo) {
    const corUm = corDoTipo(primeiroTipo);
    if (!segundoTipo) {
        return `linear-gradient(90deg, ${corUm} 0%, ${corUm} 100%)`;
    }
    const corDois = corDoTipo(segundoTipo);
    return `linear-gradient(90deg, ${corUm} 0%, ${corUm} 42%, ${corDois} 58%, ${corDois} 100%)`;
}

/** Fundo suave atrás do sprite, puxando as cores do(s) tipo(s). */
function fundoTipos(primeiroTipo, segundoTipo) {
    const corUm = corDoTipo(primeiroTipo);
    const corDois = segundoTipo ? corDoTipo(segundoTipo) : corUm;
    return `linear-gradient(135deg, ${corUm}22 0%, ${corDois}33 100%)`;
}

/** Devolve o HTML das etiquetas de tipo (uma ou duas). */
function badgesTipo(primeiroTipo, segundoTipo) {
    return [primeiroTipo, segundoTipo]
        .filter(Boolean)
        .map(t => `<span class="badge-tipo" style="background:${corDoTipo(t)}">${nomeDoTipo(t)}</span>`)
        .join("");
}
