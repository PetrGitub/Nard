# Blueprint

## Základní elementy

- šachovnice / hrací plocha
- pole
- koordinát
- figurka
- hráč
- tah

### Board (Hra / Šachovnice)

#### **Objective**
Hlavní logika hry. Řídící mechanismus. Teoreticky může být rozděleno do více objektů, např. nějaký render object, který by hrací plochu vykresloval.

#### Data
```c#
Player[] players // Hráči
Player playerOnMove // Hráč na tahu
Piece selectedPiece // Vybrané políčko
Move[] moves // Historie tahů
```

#### Constructor
```c#
// 1. Vygenerovat hrací plochu pomocí políček
```

#### Funkce
```c#
// Init (spouštěcí funkce), úvodní nastavení hry
init() {
    // Vytvořit hráče
    // Rozmístit jejich figurky v počátečním rozložení
}

// Hlavní herní smyčka, která řídí běh programu
loop() {
    //
}

// Výběr figurky
// Return hodnota je bool, podle kterého poznáme, zda to proběhlo úspěšně a nebo ne
selectPiece() : bool {
    // Hráče na tahu známe
    // Kontrola (funkce)
        // 1. Je na této souřadnici figurka?
        // 2. Je to figurka hráče na tahu?
        // 3.a Pokud ano, přiřadíme ji do Board.selectedPiece
        // 3.b Pokud ne, tak co se stane? (error hláška a opakujeme výběr)

}
```

### Cell (Políčko)

#### Objective
Udržovat **souřadnici** a také **figurku**, pokud se zrovna na poli nachází.

#### Data
```c#
Coord coord // Coordinate
string color // 'black' / 'white'
```


### Coord (Koordinát)

#### Objective
Udržovat hodnoty **x** a **y** souřadnic. Jedná se tedy o data object, (objekt, která nemá žádné funkce, pouze udržuje svou hodnotu).

#### Data
```c#
int x
int y
```

### Piece (Figurka)

#### Objective
S figurkou budeme moct provádět tahy. Figurka náleží **hráči** a má jeho barvu.

#### Data
```c#
int x
int y
```

#### Funkce
```c#
// Tah s figurkou
// Parametrem je koordinát destinace
// Return hodnota je Move nebo null, podle kterého poznáme, zda to proběhlo úspěšně a nebo ne a také, abychom mohli případně vrácený tah zaznamenat do historie
move(Coord toCoor) : Move|null {
    // 1. Kontrola (checkPossibleMove)

    // 2.a Pokud ne, tak return null + error hláška a opakujeme tah

    // 2.b Pokud ano, tak figurku přemístíme a zároveň musíme zkontrolovat, jestli jsme náhodou nevyhodili soupeřovy figurky, které uložíme do proměnné eliminatedPieces = (doDamage()) Taky musíme zaznamenat tah do historie tahů (return makeMove()).
}

// Vyhazování figurek soupeře
// Vrací pole vyhozených figurek
doDamage(): Piece[] {
    // 1. Jsou okola nás ortogonálně nějaké soupeřovy figurky?

    // 2.a Pokud ne, tak return false

    // 2.b Pokud ano
        // Je ve stejném směru ob jednu figurku další naše figurka?
        // 2.b.i Pokud ano
            // Přidáme ji do proměnné eliminatedPieces
        // 2.b.ii Pokud ne, tak nic
}

// Vrací nový provedený tah
makeMove(Coord from, Coord to, Player player, Piece[] eliminatedPieces): Move {
    // return new Move(from, to, player, eliminatedPieces)
}

// Kontrola možného tahu
checkPossibleMove(Piece piece, Coord toCoord): bool {
    // 1. Je toto pole volné?
    // 2. Je toto pole v ortogonálním směru a zároveň přímo sousedící?
}

```

### Player (Hráč)

#### Objective
Udržovat informaci o tom, co je to za **typ** hráče a také zda-li je zrovna na tahu a případně jaké má **skóre**. Dále také na začátku hry rozdělíme **figurky** mezi hráče, bude tedy udržovat i pole svých **figurek**.

#### Data
```c#
string type // 'player' / 'computer'
string color // 'blue' / 'red'
Piece[] pieces // pole figurek
int score // můžeme a nemusíme implementovat
```


### Move (Tah)

#### Objective
Entita, kterou využijeme do historie tahů. Udržuje pár koordinátů **from** pole x **to** pole y.

#### Data
```c#
Coord from // from Coordinate
Coord to // to Coordinate
Player player // Kdo tah provedl
Piece[] eliminatedPieces // Vyhozené figurky
```
