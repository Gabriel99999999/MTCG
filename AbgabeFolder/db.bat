docker stop MTCGServer 
docker rm MTCGServer 
docker run -d --name MTCGServer -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=123 -p 10002:10002 postgres

timeout /t 10 /nobreak

docker exec -i MTCGServer createdb -U postgres MTCGServer 