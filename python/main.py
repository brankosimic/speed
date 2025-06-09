from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List, Optional
from db import film, session_local
from sqlalchemy import select, insert, update, delete

app = FastAPI()

API_PREFIX = "/api"

class FilmBase(BaseModel):
    title: str
    description: str = ""
    release_year: int
    language_id: int
    original_language_id: Optional[int] = None
    rental_duration: int
    rental_rate: float
    length: int
    replacement_cost: float
    rating: str = ""
    last_update: str  # Use datetime if you want: datetime.datetime
    special_features: List[str] = []
    fulltext: Optional[str] = None  # Or use a more specific type if needed

class FilmCreate(FilmBase):
    pass

class Film(FilmBase):
    film_id: int
    class Config:
        orm_mode = True

@app.get(f"{API_PREFIX}/films", response_model=List[Film])
def get_films():
    with session_local() as session:
        result = session.execute(select(film)).fetchall()
        return [Film(**dict(row._mapping)) for row in result]

@app.get(f"{API_PREFIX}/films/{{film_id}}", response_model=Film)
def get_film(film_id: int):
    with session_local() as session:
        result = session.execute(select(film).where(film.c.film_id == film_id)).first()
        if not result:
            raise HTTPException(status_code=404, detail="Film not found")
        return Film(**dict(result._mapping))

@app.post(f"{API_PREFIX}/films", response_model=Film)
def create_film(film_data: FilmCreate):
    with session_local() as session:
        stmt = insert(film).values(**film_data.dict()).returning(film)
        result = session.execute(stmt)
        session.commit()
        return Film(**dict(result.first()._mapping))

@app.put(f"{API_PREFIX}/films/{{film_id}}", response_model=Film)
def update_film(film_id: int, film_data: FilmCreate):
    with session_local() as session:
        stmt = update(film).where(film.c.film_id == film_id).values(**film_data.dict()).returning(film)
        result = session.execute(stmt)
        session.commit()
        updated = result.first()
        if not updated:
            raise HTTPException(status_code=404, detail="Film not found")
        return Film(**dict(updated._mapping))

@app.delete(f"{API_PREFIX}/films/{{film_id}}")
def delete_film(film_id: int):
    with session_local() as session:
        stmt = delete(film).where(film.c.film_id == film_id).returning(film)
        result = session.execute(stmt)
        session.commit()
        deleted = result.first()
        if not deleted:
            raise HTTPException(status_code=404, detail="Film not found")
        return {"detail": "Film deleted"}

@app.get("/")
async def read_root():
    return {"message": "Hello, World!"}

@app.get("/items/{item_id}")
async def read_item(item_id: int, q: str = None):
    return {"item_id": item_id, "q": q}