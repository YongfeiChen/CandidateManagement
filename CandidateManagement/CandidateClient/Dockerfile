# Build stage
FROM node:20-alpine AS builder

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm install

COPY . .
RUN npm run build

# Runtime stage
FROM node:20-alpine

WORKDIR /app

RUN npm install -g http-server

COPY --from=builder /app/dist/candidate-client/browser ./dist

EXPOSE 3000

CMD ["http-server", "dist", "-p", "3000", "--gzip", "-c-1"]

