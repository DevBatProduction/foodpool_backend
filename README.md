# Foodpool
---
## Describe
**Foodpool** comes from a combination of the words **food** and **pool**, as the app is based on the idea of pooling resources to make food delivery more convenient and cost-effective.

The term **pool** is often used in the context of carpooling, where people share a ride in a car to save money and reduce their environmental impact. In the same way, **Foodpool** suggests a shared resource for food delivery, where people can come together to make group orders.

The name **Foodpool** is catchy and easy to remember, which is important for an app that aims to simplify the food delivery process. It also conveys the sense of community and collaboration that is at the heart of the app's mission.

## Installation 🔨

### Install All Library In Project
```bash
dotnet restore
```

### Install Database Migration Tool
```bash
dotnet tool install --global dotnet-ef
```

## Start 🔥

### Run without Hot Reload
```bash
dotnet run
```

### Run with Hot Reload
```bash
dotnet watch
```

## Other 🔍

### Database Add Migration
```bash
dotnet ef migrations add {MigrationName} -c FoolpoolDbContext -o ./data/migrations
```
#### MigrationName is a name of action did you do.

### Database Update Migration
```bash
dotnet ef database update
```