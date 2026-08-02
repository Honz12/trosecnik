# Trosečník

Tile-based 2D přeživší hra s grafikou v Raylib-cs. Inspirována filmem *Trosečník* a hrou *The Forest*.

## 🎮 O hře

Hra začíná černou obrazovkou, která se postupně zesvětlí a odhalí hráče na ostrově uprostřed Atlantiku. Cílem je přežít na ostrově – hlídat si vodu, hlad a zdraví, postavit si obydlí, ubránit se kanibalům a nakonec uniknout z ostrova pomocí vrtulníku nebo lodi.

### Herní prvky

- **Přežití** – ukazatele vody, hladu a zdraví
- **Stavění** – stan (vylepšitelný), oheň, loučeň, SOS signál
- **Prostředí** – velký ostrov s palmami a zvířaty (zajíci, slepice, ryby)
- **Nepřátelé** – kanibalové žijící na opačné straně ostrova (chování inspirováno *The Forest*)
- **Záchrana** – po 50–100 dnech se objeví vrtulník/loď, minihra upoutání pozornosti s RNG šancí

## 🛠️ Požadavky

- [.NET 10.0](https://dotnet.microsoft.com/) (nebo novější)
- Raylib-cs

## 🚀 Spuštění

```bash
dotnet restore
dotnet run
```

## 📂 Struktura projektu

```
trosecnik/
├── src/
│   └── Program.cs        # vstupní bod hry
├── PLAN.md               # herní plán a vývojový checklist
└── trosecnik.csproj      # konfigurace projektu
```

## 📅 Vývoj

Podrobný plán a seznam úkolů najdeš v souboru [PLAN.md](PLAN.md).

## 📄 Licence

Tento projekt je licencován pod [licencí MIT](LICENSE).
