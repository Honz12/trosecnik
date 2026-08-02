# Plán pro hru Trosečník

---

## 1. Základní informace

- **Typ hry:** Tile-based 2D hra
- **Grafika:** Raylib-cs
- **Inspirace:** Film *Trosečník* a hra *The Forest*
- **Cíl hráče:** Přežít na ostrově a uniknout z něj

---

## 2. Herní příběh a prostředí

- Hra začíná černou obrazovkou, která se postupně zesvětlí a odhalí ostrov uprostřed Atlantiku.
- Ostrov je poměrně velký a obsahuje:
  - **Prostředí:** palmy a další vegetace
  - **Zvířata:** zajíci, slepice, ryby aj.
  - **Domorodce:** kanibalové žijící na opačné straně ostrova (chování podobné jako ve hře *The Forest*)

---

## 3. Herní mechaniky

### 3.1 Základní přežití
Hráč musí hlídat tři ukazatele:
- **Žízeň**
- **Hlad**
- **Zdraví**

### 3.2 Stavění a obydlí
- **Stan** – základní obydlí, které lze postupně vylepšovat.
- Ve stanu je hráč v bezpečí:
  - Přečká v něm noc
  - Může se schovat před kanibaly
- Další stavitelné věci:
  - **SOS signál** – vyskládaný z kamínků na pláži
  - **Signalizační oheň** – zvyšuje šanci na záchranu
  - **Normální oheň**
  - **Loučeň**

### 3.3 Časový průběh a záchrana
- Po přežití dostatečného počtu dnů (např. **50–100 dní**) začne být šance na záchranu.
- Kolem ostrova může přeletět **vrtulník** nebo proplout **loď**.
- **Signalizační oheň a SOS signál** zvyšují šanci na záchranu.
- Po objevení možnosti "extrakce" se spustí **minihra**:
  1. Hráč se snaží upoutat pozornost posádky.
  2. Předem je stanovená náhodná šance na úspěch (pomocí RNG).
  3. Minihra šanci **zvětšuje nebo zmenšuje** (např. jako v *fishing-cs* – zelené pole šanci zvyšuje, červené snižuje). **(Klidně můžeme vymyslet jiný systém)**
  4. Po uplynutí náhodného času (např. **10–20 s**, určeno RNG) se podle výsledné šance rozhodne, jestli si posádka hráče všimne a odveze ho.
  5. Pokud hráč není zachráněn, musí čekat dál – **cooldown** mezi pokusy (např. **1–3 dny, opět RNG**).

---

## 4. Plánované funkce (checklist)

### Fáze 1 – Základ hry
- [ ] Nastavení projektu s Raylib-cs
- [ ] Vykreslení ostrova jako tile-map
- [ ] Pohyb hráče
- [ ] HUD s ukazateli (voda, hlad, zdraví)

### Fáze 2 – Přežití
- [ ] Ztráta hladu / vody / zdraví v čase
- [ ] Sběr jídla a vody (ryby, zajíci, slepice)
- [ ] Systém dne a noci

### Fáze 3 – Stavění
- [ ] Základní stavění stanu
- [ ] Vylepšování stanu
- [ ] Oheň (normální + signalizační)
- [ ] SOS signál na pláži
- [ ] Loučeň

### Fáze 4 – Nepřátelé
- [ ] Kanibalové (AI (úplně primitivní) podobná *The Forest*)
- [ ] Útoky na hráče a obydlí

### Fáze 5 – Záchrana
- [ ] Spawning vrtulníku / lodi
- [ ] Minihra upoutání pozornosti
- [ ] Systém šancí a cooldownu
- [ ] Vítězný (záchranný) konec hry

---

## 5. Další nápady (k zvážení)

- [ ] Vnitřní inventář / craftování
- [ ] Dešťová voda (sběr pitné vody)
- [ ] Zranění a léčení
- [ ] Uložení hry (save systém)
- [ ] Zvuky a hudba
