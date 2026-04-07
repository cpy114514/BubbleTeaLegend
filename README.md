<<<<<<< HEAD
Bubble Tea Legend
Bubble Tea Legend is a 2D survival action game where the drink you craft becomes your fighter.

Mix milk, red or green tea, and toppings like pearls to create your own build.  
Enter the battlefield and survive endless waves of enemies.  
Every ingredient changes how your tea fights — no two cups battle the same way.

How to Play

Craft Your Drink
Start by making your bubble tea.
Add milk, red tea or green tea, and choose your toppings like pearls.
Your recipe determines how you fight.

Enter the Battle
Once your drink is ready, step into the arena.
Your character attacks automatically.

Stay Within Range
You will automatically target the nearest enemy within your attack range.
Positioning matters — move carefully.

Survive the Waves
Enemies will continuously spawn and grow stronger over time.
Avoid getting surrounded.

Watch Your Health
Your health is shown as liquid inside your cup.
When the cup runs empty, the game is over.
=======
# Bubble Tea Maker

`Bubble Tea Maker` is a Unity game with two phases:

1. Build a custom bubble tea in the maker scene.
2. Take that drink into combat, where toppings become weapons.

## Optimization

This submission implements two optimization techniques required by the sidequest: efficient algorithms/caching and reduced memory usage.

### 1. Combat Target Caching

What I improved:

- Enemy targeting in `PearlAttack`, `GrapeAttack`, `CoconutAttack`, `LemonAttack`, `PearlProjectile`, and `PuddingControl`
- Pudding separation logic
- Enemy-to-player damage lookups in `EnemyMoveAI` and `EnemyBullet`
- Enemy crowd separation in `EnemyMoveAI`

How I improved it:

- Added a shared `EnemyRegistry` that tracks active enemies through `Enemy.OnEnable()` and `Enemy.OnDisable()`.
- Replaced scene-wide `FindObjectsOfType<Enemy>()` scans with `EnemyRegistry.GetNearest(...)`.
- Replaced `FindObjectsOfType<PuddingControl>()` with an active pudding list.
- Replaced `FindObjectOfType<PlayerHealthParticles>()` with `PlayerHealthParticles.Instance`.
- Replaced one hot-path `Physics2D.OverlapCircleAll(...)` allocation with `Physics2D.OverlapCircleNonAlloc(...)`.

Results:

- Removed `8` combat-time `FindObjectsOfType<Enemy>()` scene scans.
- Removed `1` `FindObjectsOfType<PuddingControl>()` scan from pudding separation.
- Removed `2` `FindObjectOfType<PlayerHealthParticles>()` lookups from enemy damage code.
- Removed array allocation from enemy crowd separation by switching to `OverlapCircleNonAlloc`.

### 2. Health UI Object Pooling

What I improved:

- The player HP blood-particle UI in `PlayerHealthParticles`

How I improved it:

- The old version destroyed every existing blood particle and re-instantiated the full bar on every health change.
- The new version keeps a particle pool, grows it only when needed, and reuses objects by toggling them active/inactive.

Results:

- `maxHP` is `100`, so draining the player from full HP to `0` used to cause `5,050` instantiates and `5,050` destroys in the HUD path.
- After pooling, the same path instantiates at most `100` blood particles once and then reuses them.
- A single 1-damage hit at full health now avoids `199` object lifecycle operations (`100` destroys + `99` instantiates).

## Verification

- `dotnet build Assembly-CSharp.csproj -nologo`
- Result: build succeeded with `0` errors

## Manual Submission Steps

- Ship the project on Flavortown
- Submit it to the Optimization sidequest from the project page
>>>>>>> 79ede0b (update)
