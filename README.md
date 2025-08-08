# Zápočtový program – Swarm Intelligence (Boids)

**Jméno studenta:** Lukáš Stibor  
**Předmět:** Programování 2  
**Datum:** 25. 4. 2025  
**Jazyk:** C#  
**Technologie:** .NET, WPF

---

## 1. Idea a základní popis 

Cílem programu je vytvořit simulaci kolektivního chování hejna pomocí modelu "boids", který je inspirován přirozeným pohybem ptáků nebo ryb v hejnu. Jednoduší agenti (tzv. boidi) se pohybují podle několika jednoduchých pravidel a společně vykazují komplexní skupinové chování – tzv. swarm intelligence.

---

## 2. Formalizace problému

Každý boid je entita v 2D prostoru se stavem definovaným pozicí, směrem a rychlostí. Jeho chování je určeno třemi pravidly:
- **Separace:** boid se vyhýbá příliš blízkým sousedům.
- **Zarovnání:** boid se orientuje podle směru ostatních boidů v okolí.
- **Soudržnost:** boid se snaží držet blízko středu svého okolního hejna.

---

## 3. Základní návrh algoritmu

1. Inicializace `n` boidů s náhodnými pozicemi a rychlostmi.
2. V každém framu se provede:
    - Pro každý boid se identifikují sousedé v určité vzdálenosti.
    - Aplikují se základní pravidla (separace, zarovnání, soudržnost).
    - Vypočítá se výsledný pohybový vektor.
    - Aktualizuje se pozice a směr boida.
3. Výsledný stav se vykreslí v grafickém rozhraní.

---

## 4. Forma a popis vstupů a výstupů

### Výstupy:
- Vizualizace aktuálních pozic a směrů boidů v reálném čase.
- Export boidů ve foramátu, tak aby to šlo i importovat
- Volitelné: počet boidů, souřadnice kurzoru.

### Vstupy:
- Import boidů připravených na pozicích
- Volitelné: Počet boidů

---

## 5. Forma interfacu

- **Rozhraní:** WPF aplikace
- **GUI prvky:**
  - Oblast pro vykreslení simulace 
  - Tlačítka: Start, Pause, Reset, Import, Export
  - Slidery pro nastavení parametrů (pokud budou vstupní parametry)

---
