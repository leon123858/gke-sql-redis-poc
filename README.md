# gke-sql-redis-poc

## deploy test image

```bash
docker build -t gke-poc . -f gke-poc/Dockerfile
docker tag gke-poc leon1234858/gke-poc:arm64
docker push leon1234858/gke-poc:arm64
```

```bash
docker buildx build --platform linux/amd64 -t leon1234858/gke-poc:lastest -f gke-poc/Dockerfile .
docker tag gke-poc leon1234858/gke-poc:lastest
docker push leon1234858/gke-poc:lastest
```
